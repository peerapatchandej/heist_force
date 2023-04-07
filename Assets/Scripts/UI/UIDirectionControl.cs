using UnityEngine;

namespace FenrirStudio.HeistForce
{
    public class UIDirectionControl : MonoBehaviour
    {
        #region Public Varaibles

        public bool m_UseRelativeRotation = true; 

        #endregion

        #region Private Variables

        private Quaternion m_RelativeRotation; 

        #endregion

        #region MonoBehaviour Callback

        private void Start()
        {
            m_RelativeRotation = transform.parent.localRotation;
        }


        private void Update()
        {
            if (m_UseRelativeRotation)
            {
                transform.rotation = m_RelativeRotation;
            }
        }

        #endregion
    }
}