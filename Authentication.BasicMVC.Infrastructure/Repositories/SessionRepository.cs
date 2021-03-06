﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Dapper;
using Authentication.BasicMVC.Domain.Models;
using Authentication.BasicMVC.Domain.Repositories;
//using Authentication.BasicMVC.Infrastructure.Interfaces;
using BasicMVC.Core.Data.Interfaces;

namespace Authentication.BasicMVC.Infrastructure.Repositories
{
  public class SessionRepository : ISessionRepository
  {

    //private readonly IDbContext CurrentContext;
    private readonly LoginList sessions;

    public SessionRepository(LoginList currentSessions)
    {
      if (currentSessions == null)
        throw new ArgumentNullException("connectionString");

      this.sessions = currentSessions;
    }

    //public SessionRepository()
    //{
    //  this.CurrentContext = DbContext.Create();
    //}

    public void Dispose()
    {

    }

    public virtual Task CreateAsync(ClientSession clientSession)
    {
      if (clientSession == null)
        throw new ArgumentNullException("clientSession");
      var owner = this.FindByClientAsync(clientSession.ClientSessionID);
      if ((owner == null) || (owner.Result == null))
      {
        return Task.Factory.StartNew(() =>
        {
          clientSession.Id = Guid.NewGuid();
          sessions.ClientSessions.Add(clientSession);
          //IDbConnection connection = CurrentContext.OpenConnection(CurrentContext.CurrentTransaction);
          //connection.Execute("insert into auth_ClientSessions(Id, LocalSessionID, ClientSessionID, LoginID) values(@Id, @LocalSessionID, @ClientSessionID, @LoginID)", clientSession, CurrentContext.CurrentTransaction);
        });
      }
      else
      {
        clientSession.Id = owner.Result.Id;
        this.UpdateAsync(clientSession);
        return Task.FromResult(0);
      }
    }

    public virtual Task DeleteAsync(ClientSession clientSession)
    {
      if (clientSession == null)
        throw new ArgumentNullException("clientSession");

      return Task.Factory.StartNew(() =>
      {
        sessions.ClientSessions.Remove(sessions.ClientSessions.Where(x => x.Id==clientSession.Id).SingleOrDefault<ClientSession>());
        //IDbConnection connection = CurrentContext.OpenConnection(CurrentContext.CurrentTransaction);
        //connection.Execute("delete from auth_ClientSessions where Id = @Id", new { clientSession.Id }, CurrentContext.CurrentTransaction);
      });
    }

    public virtual Task UpdateAsync(ClientSession clientSession)
    {
      if (clientSession == null)
        throw new ArgumentNullException("user");
      if(clientSession.LoginID == Guid.Empty)
        clientSession.LoginID = null;
      
      var currentSession = FindByIdAsync(clientSession.Id);

      return Task.Factory.StartNew(() =>
      {
        if((currentSession!=null)&&(currentSession.Result!=null))
        {
          currentSession.Result.LocalSessionID=clientSession.LocalSessionID;
          currentSession.Result.ClientSessionID = clientSession.ClientSessionID;
          currentSession.Result.LoginID = clientSession.LoginID;
        }
        //IDbConnection connection = CurrentContext.OpenConnection(CurrentContext.CurrentTransaction);
        //connection.Execute("update auth_ClientSessions set LocalSessionID=@LocalSessionID, ClientSessionID=@ClientSessionID, LoginID=@LoginID where ID = @ID", clientSession, CurrentContext.CurrentTransaction);
      });
    }

    public virtual Task<ClientSession> FindByIdAsync(Guid Id)
    {
      if (Id == Guid.Empty)
        throw new ArgumentNullException("Id");

      return Task.Factory.StartNew(() =>
      {
        return sessions.ClientSessions.Where(x => x.Id==Id).SingleOrDefault<ClientSession>();
        //using(IDbConnection connection = CurrentContext.OpenConnection())
        //return connection.Query<ClientSession>("select * from auth_ClientSessions where Id = @Id", new { Id = Id }).SingleOrDefault();
      });
    }

    public virtual Task<ClientSession> FindByClientAsync(Guid clientSessionID)
    {
      if (clientSessionID == Guid.Empty)
        throw new ArgumentNullException("clientSessionID");

      return Task.Factory.StartNew(() =>
      {
        return sessions.ClientSessions.Where(x => x.ClientSessionID == clientSessionID && x.LoginID==null).SingleOrDefault<ClientSession>();
        //using(IDbConnection connection = CurrentContext.OpenConnection())
        //return connection.Query<ClientSession>("select * from auth_ClientSessions where ClientSessionID = @ClientSessionId AND LoginID IS NULL", new { ClientSessionId = clientSessionID }).SingleOrDefault();
      });
    }

    public virtual Task<ClientSession> FindByClientAndSessionAsync(Guid clientSessionID, string localSessionID)
    {
      if (clientSessionID == Guid.Empty)
        throw new ArgumentNullException("clientSessionID");

      return Task.Factory.StartNew(() =>
      {
        return sessions.ClientSessions.Where(x => x.ClientSessionID == clientSessionID && x.LocalSessionID==localSessionID).SingleOrDefault<ClientSession>();
        //using(IDbConnection connection = CurrentContext.OpenConnection())
        //return connection.Query<ClientSession>("select * from auth_ClientSessions where LocalSessionID = @ClientSessionId AND ClientSessionID = @LocalSessionId AND LoginID IS NULL", new { ClientSessionId = clientSessionID, LocalSessionId = localSessionID }).SingleOrDefault();
      });
    }

    public virtual Task AssignLoginToSessions(string sessionId, Login login)
    {
      if (sessionId == "")
        throw new ArgumentNullException("clientSession");
      if (login == null)
        throw new ArgumentNullException("Login");
      if (login.Id == Guid.Empty)
      { 
        throw new ArgumentNullException("Login Id: " + login.Id.ToString());
      }
      return Task.Factory.StartNew(() =>
      {
        ClientSession currentSession = sessions.ClientSessions.Where(x => x.LocalSessionID==sessionId && x.LoginID==null).SingleOrDefault<ClientSession>();
        if(currentSession!=null)
        {
          currentSession.LoginID = login.Id;
        }
        //IDbConnection connection = CurrentContext.OpenConnection(CurrentContext.CurrentTransaction);
        //connection.Execute("Update auth_ClientSessions SET LoginID=@LoginID where LocalSessionID = @LocalSessionID AND LoginID IS NULL", new { LoginID = login.Id, LocalSessionID = sessionId }, CurrentContext.CurrentTransaction);
      });
    }
  }
}
