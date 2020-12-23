using System.Web;
using System.Web.Mvc;

namespace DV.FrenteLoja.Helpers
{
    public static class StepControlHelper
    {
        public static HtmlString StepControl(this HtmlHelper htmlHelper, int step, int maxSteps)
        {
            string htmlResult = "<div class='containerSteps'>"; 

            for(int i = 1; i < maxSteps + 1; i++)
            {
               
                if (i <= step)
                    htmlResult += $@"<div class='stepBlock active'>";
                else
                    htmlResult += $@"<div class='stepBlock'>";

                htmlResult += $@"<div class='separator2'>
                        <div class='line'></div>
                    </div>
                    <div class='step'>
                        {i}
                    </div>
                    </div>";
                
            }

            htmlResult +="</div>";

            htmlResult.Replace("'", "\"");

            return new HtmlString(htmlResult);
        }
    }
}