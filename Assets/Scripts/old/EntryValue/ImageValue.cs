using UnityEngine;

namespace EntryValue
{
	public class ImageValue : EntryValue
	{
#if UNITY_EDITOR
		private void OnEnable()
		{
			if (writerImageObj == null && readerImageObj == null) {
				writerImageObj = GetComponent<OpenImageUploadPanelScript>();
				readerImageObj = GetComponent<ReaderOpenImageUploadPanelScript>();
			}
		}
#endif

		public OpenImageUploadPanelScript writerImageObj;
		public ReaderOpenImageUploadPanelScript readerImageObj;

		public override void SetValue(string text)
		{
			if (writerImageObj != null) {
				writerImageObj.SetGuid(text);
				writerImageObj.LoadData(text);
			} else if (readerImageObj != null) {
				readerImageObj.SetGuid(text);
				readerImageObj.LoadData(text);
			}
		}

		public override string GetValue()
		{
			if (writerImageObj != null) {
				return writerImageObj.GetGuid();
			} else if (readerImageObj != null) {
				return readerImageObj.GetGuid();
			}
			throw new System.Exception("No image object defined");
		}

		public override string GetTag()
		{
			return "Image";
		}

		public override bool MatchConditions(GameObject obj)
		{
			return obj.GetComponent<OpenImageUploadPanelScript>() != null ||
				obj.GetComponent<ReaderOpenImageUploadPanelScript>() != null;
		}
	}
}