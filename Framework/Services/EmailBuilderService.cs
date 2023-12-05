using Framework.DomainModels;
using Framework.DomainModels.Base;
using Framework.Extensions;

namespace Framework.Services
{
    public class EmailBuilderService
    {
        public string BuildErinnerungMailMessage(ToDoItemDomainModel toDoItem, ToDoListDomainModel list, DateTime nextTime, bool onlyOnce = true)
        {
            if (onlyOnce)
            {
                return $@"<html><body>
                    <div>
                        <div style='font-weight:bold'>{toDoItem.Bezeichnung}! </div>
                        <div>(Liste ""{list.Name}"": <a href=""{list.Id.ToListUrl()}"">hier ansehen</a>, Schedule: <a href=""{toDoItem.Id.ToScheduleEditUrl()}"">hier ändern</a>)</div>
                    </div>
                    <div>
                        <span>
                            Bitte bis <span style='font-weight:bold'>{nextTime.ToShortDateString()}</span> um <span style='font-weight:bold'>{nextTime.ToShortTimeString()}!</span> erledigen.
                        </span>
                    </div></body></html>";
            }
            else
            {
                return $@"<html><body>
                    <div>
                        <div style='font-weight:bold'>{toDoItem.Bezeichnung}! </div>
                        <div>(Liste ""{list.Name}"": <a href=""{list.Id.ToListUrl()}"">hier ansehen</a>, Schedule: <a href=""{toDoItem.Id.ToScheduleEditUrl()}"">hier ändern</a>)</div>
                    </div>
                    <div>
                        <span>
                            Bitte bis zur nächsten Fälligkeit <span style='font-weight:bold'>{nextTime.ToShortDateString()}</span> um <span style='font-weight:bold'>{nextTime.ToShortTimeString()}!</span> erledigen.
                        </span>
                    </div></body></html>";
            }
        }

        public string BuildInvitationMailMessage(string url) => $@"
            <html>
                <body>
                    <div>
                        <span>
                            Sie wurden in die Applikation todoos.net eingeladen: <br>Bei Interesse bitte <a href=""{url}"">hier anmelden</a> :).
                        </span>
                    </div>
                </body>
            </html>";
    }
}