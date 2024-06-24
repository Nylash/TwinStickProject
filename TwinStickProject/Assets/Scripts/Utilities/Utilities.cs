using UnityEngine;

public static class Utilities 
{
    #region METHODS
    public static void SetChildsLayers(GameObject go, int layerNumber)
    {
        if (go == null) return;
        foreach (Transform trans in go.GetComponentsInChildren<Transform>(true))
        {
            trans.gameObject.layer = layerNumber;
        }
    }
    #endregion

    #region ENUMS

    #endregion
}
