using UnityEngine;
using UnityEngine.Networking;


namespace ClinicalTools.SimEncounters.MainMenu
{
    public class ResendEmail
    {
        protected IWebAddress WebAddress { get; }
        protected MessageHandler MessageHandler { get; }
        protected ServerDataReader<string> RegisterDataReader { get; }
        public ResendEmail(IWebAddress webAddress, MessageHandler messageHandler)
        {
            WebAddress = webAddress;
            MessageHandler = messageHandler;
            var statusesParser = new StringParser();
            RegisterDataReader = new ServerDataReader<string>(statusesParser);
        }

        private const string phpFile = "Login.php";
        private const string actionVariable = "ACTION";
        private const string resendAction = "resendActivation";
        private const string emailVariable = "email";

        /**
         * Downloads all available and applicable menu files to display on the main manu.
         * Returns them as a MenuCase item
         */
        public void Resend(string email)
        {
            var url = WebAddress.GetUrl(phpFile);
            var form = CreateForm(email);

            var webRequest = UnityWebRequest.Post(url, form);
            RegisterDataReader.Completed += EncounterDataReader_Completed;
            RegisterDataReader.Begin(webRequest);
        }

        public WWWForm CreateForm(string email)
        {
            var form = new WWWForm();

            form.AddField(actionVariable, resendAction);

            form.AddField(emailVariable, email);

            return form;
        }

        private const string errorSuffix = "--Could not send email";
        private void EncounterDataReader_Completed(object sender, ServerResult<string> e)
        {
            if (e.Outcome != ServerOutcome.Success) { 
                MessageHandler.ShowMessage(e.Message, true);
                return;
            } 
            if (!e.Result.EndsWith(errorSuffix)) { 
                MessageHandler.ShowMessage(e.Result, false);
                return;
            }

            var error = e.Result.Substring(0, e.Result.Length - errorSuffix.Length);
            MessageHandler.ShowMessage(error, true);
        }


    }
}
