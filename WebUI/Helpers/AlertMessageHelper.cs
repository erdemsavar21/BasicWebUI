using System;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace WebUI.Helpers
{
    public class AlertMessageHelper
    {

      
        public static void Success(Controller controller,string message, bool dismissable = false)
        {
            AddAlert(controller,AlertStyles.Success, message, dismissable);
        }

        public static void Information(Controller controller,string message, bool dismissable = false)
        {
            AddAlert(controller,AlertStyles.Information, message, dismissable);
        }

        public static void Warning(Controller controller,string message, bool dismissable = false)
        {
            AddAlert(controller,AlertStyles.Warning, message, dismissable);
        }

        public static void Danger(Controller controller,string message, bool dismissable = false)
        {
            AddAlert(controller,AlertStyles.Danger, message, dismissable);
        }

        private static void AddAlert(Controller controller,string alertStyle, string message, bool dismissable)
        {

           
            var alerts = controller.TempData.ContainsKey(Alert.TempDataKey)
                ? (List<Alert>)controller.TempData[Alert.TempDataKey]
                : new List<Alert>();

            alerts.Add(new Alert
            {
                AlertStyle = alertStyle,
                Message = message,
                Dismissable = dismissable
            });

            controller.TempData[Alert.TempDataKey] = alerts;
        }
    }
}
