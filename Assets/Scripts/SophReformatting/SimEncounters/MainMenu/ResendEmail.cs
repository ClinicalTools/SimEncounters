using UnityEngine;
using UnityEngine.Networking;


namespace ClinicalTools.SimEncounters.MainMenu
{
    public class ResendEmail
    {
        protected IUrlBuilder WebAddress { get; }
        protected MessageHandler MessageHandler { get; }
        protected ServerDataReader<string> ServerDataReader { get; }
        public ResendEmail(IUrlBuilder webAddress, MessageHandler messageHandler)
        {
            WebAddress = webAddress;
            MessageHandler = messageHandler;
            var statusesParser = new StringParser();
            ServerDataReader = new ServerDataReader<string>(statusesParser);
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
            var url = WebAddress.BuildUrl(phpFile);
            var form = CreateForm(email);

            var webRequest = UnityWebRequest.Post(url, form);
            ServerDataReader.Completed += EncounterDataReader_Completed;
            ServerDataReader.Begin(webRequest);
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
