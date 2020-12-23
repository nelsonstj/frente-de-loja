using System;

namespace DV.FrenteLoja.Core.Exceptions
{
    public class ServicoIntegracaoException:Exception
    {
        public ServicoIntegracaoException(string msgm):base(msgm)
        {
            
        }

        public ServicoIntegracaoException(string msg, Exception innerException) : base(msg,innerException)
        {

        }


    }
}
