using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Authentication.BasicMVC.Domain
{
  public interface IUnitOfWork
  {

    bool BeginWork();

    bool CommitWork();
  }
}
