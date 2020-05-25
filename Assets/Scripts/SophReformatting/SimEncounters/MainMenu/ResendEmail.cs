using UnityEngine;
using UnityEngine.Networking;


namespace ClinicalTools.SimEncounters.MainMenu
{
    public class ResendEmail
    {
        protected IUrlBuilder WebAddress { get; }
        protected global::MessageHandler MessageHandler { get; }
        protected IServerReader ServerReader { get; }
        public ResendEmail(IUrlBuilder webAddress, global::MessageHandler messageHandler, IServerReader serverReader)
        {
            WebAddress = webAddress;
            MessageHandler = messageHandler;
            ServerReader = serverReader;
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
            var serverResult = ServerReader.Begin(webRequest);
            serverResult.AddOnCompletedListener(ProcessResults);
        }

        public WWWForm CreateForm(string email)
        {
            var form = new WWWForm();

            form.AddField(actionVariable, resendAction);

            form.AddField(emailVariable, email);

            return form;
        }

        private const string errorSuffix = "--Could not send email";
        private void ProcessResults(ServerResult serverResult)
        {
            if (serverResult.Outcome != ServerOutcome.Success) {
                MessageHandler.ShowMessage(serverResult.Message, true);
                return;
            } 
            if (!serverResult.Message.EndsWith(errorSuffix)) {
                MessageHandler.ShowMessage(serverResult.Message, false);
                return;
            }

            var error = serverResult.Message.Substring(0, serverResult.Message.Length - errorSuffix.Length);
            MessageHandler.ShowMessage(error, true);
        }
    }
}
