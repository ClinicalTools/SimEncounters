using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConfirmationScript : MonoBehaviour {

	public delegate void DelegateMethod(GameObject obj);
	public DelegateMethod MethodToConfirm;
	public delegate void AnyParamsDelegateMethod(params object[] args);
	public AnyParamsDelegateMethod AnyParamMethodToConfirm;
	public GameObject obj;
	public object[] args;
	public TMPro.TextMeshProUGUI actionText;

	/**
	 * Used to confirm actions.
	 * Set the MethodToConfirm (or AnyParam variant) to the method that should be confirmed
	 * AnyParam will take any paramters while MethodToConfirm takes just a game object to perform an action on
	 */

	void Start() {

	}

	public void AcceptAction() {
		if (MethodToConfirm != null) {
			MethodToConfirm(obj);
		} else {
			AnyParamMethodToConfirm(args);
		}
		MethodToConfirm = null;
		AnyParamMethodToConfirm = null;
	}

	public void DestroyMe(){
		Destroy (this.gameObject);
	}
}