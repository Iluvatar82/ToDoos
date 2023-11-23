using Framework.DomainModels;
using Framework.Extensions;

namespace Framework.Services
{
    public class EmailBuilderService
    {
        public string BuildErinnerungMailMessage(ToDoItemDomainModel toDoItem, DateTime nextTime, bool onlyOnce = true)
        {
            if (onlyOnce)
            {
                return $@"
                    <div style='font-weight:bold'>
                        <span>
                            {toDoItem.Bezeichnung}! (Liste <a href=""{toDoItem.ListId.ToListUrl()}"">hier ansehen</a>, Schedule <a href=""{toDoItem.Id.ToScheduleEditUrl()}"">hier ändern</a>)
                        </span>
                    </div>
                    <div>
                        <span>
                            Bitte bis <span style='font-weight:bold'>{nextTime.ToShortDateString()}</span> um <span style='font-weight:bold'>{nextTime.ToShortTimeString()}!</span> erledigen.
                        </span>
                    </div>";
            }
            else
            {
                return $@"
                    <div style='font-weight:bold'>
                        <span>
                            {toDoItem.Bezeichnung}! (Liste <a href=""{toDoItem.ListId.ToListUrl()}"">hier ansehen</a>, Schedule <a href=""{toDoItem.Id.ToScheduleEditUrl()}"">hier ändern</a>)
                        </span>
                    </div>
                    <div>
                        <span>
                            Bitte bis zur nächsten Fälligkeit <span style='font-weight:bold'>{nextTime.ToShortDateString()}</span> um <span style='font-weight:bold'>{nextTime.ToShortTimeString()}!</span> erledigen.
                        </span>
                    </div>";
            }
        }
    }
}