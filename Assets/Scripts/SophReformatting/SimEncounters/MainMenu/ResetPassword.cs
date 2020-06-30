using UnityEngine;
using UnityEngine.Networking;


namespace ClinicalTools.SimEncounters.MainMenu
{
    public class ResetPassword
    {
        protected IUrlBuilder WebAddress { get; }
        
        protected IServerReader ServerReader { get; }
        public ResetPassword(IUrlBuilder webAddress, IServerReader serverReader)
        {
            WebAddress = webAddress;
            ServerReader = serverReader;
        }

        private const string phpFile = "Login.php";
        private const string actionVariable = "ACTION";
        private const string resetAction = "forgotPassword";
        private const string emailVariable = "email";
        private const string usernameVariable = "username";

        /**
         * Downloads all available and applicable menu files to display on the main manu.
         * Returns them as a MenuCase item
         */
        public void Reset(string email, string username)
        {
            var url = WebAddress.BuildUrl(phpFile);
            var form = CreateForm(email, username);

            var webRequest = UnityWebRequest.Post(url, form);
            var serverResult = ServerReader.Begin(webRequest);
            serverResult.AddOnCompletedListener(ProcessResults);
        }

        public WWWForm CreateForm(string email, string username)
        {
            var form = new WWWForm();

            form.AddField(actionVariable, resetAction);

            form.AddField(emailVariable, email);
            form.AddField(usernameVariable, username);

            return form;
        }

        private const string errorSuffix = "--Could not send email";
        private void ProcessResults(WaitedResult<ServerResult> serverResult)
        {
            if (serverResult.Value.Outcome != ServerOutcome.Success) {
                //MessageHandler.ShowMessage(serverResult.Message, true);
                return;
            }
            if (!serverResult.Value.Message.EndsWith(errorSuffix)) {
                //MessageHandler.ShowMessage("Success. Please check email for verification", false);
                return;
            }

            var error = serverResult.Value.Message.Substring(0, serverResult.Value.Message.Length - errorSuffix.Length);
            //MessageHandler.ShowMessage("Unable to send email", true);
        }
    }
}
