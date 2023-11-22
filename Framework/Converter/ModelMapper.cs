using AutoMapper;
using Framework.DomainModels;
using Framework.DomainModels.Base;
using ToDo.Data.ToDoData.Entities;

namespace Framework.Converter
{
    public class ModelMapper
    {
        private readonly IMapper _mapper;

        public ModelMapper(IMapper mapper) 
        {
            _mapper = mapper;
        }

        public object Map<T>(T source) where T : class
        {
            return source switch
            {
                Category category => _mapper.Map<CategoryDomainModel>(category),
                HangfireJob job => _mapper.Map<HangfireJobDomainModel>(job),
                Schedule schedule => _mapper.Map<ScheduleDomainModel>(schedule),
                ScheduleReminder reminder => _mapper.Map<ScheduleReminderDomainModel>(reminder),
                Setting setting => _mapper.Map<SettingDomainModel>(setting),
                ToDoItem item => _mapper.Map<ToDoItemDomainModel>(item),
                ToDoList list => _mapper.Map<ToDoListDomainModel>(list),
                UserGroup group => _mapper.Map<UserGroupDomainModel>(group),

                CategoryDomainModel category => _mapper.Map<Category>(category),
                HangfireJobDomainModel job => _mapper.Map<HangfireJob>(job),
                ScheduleDomainModel schedule => _mapper.Map<Schedule>(schedule),
                ScheduleReminderDomainModel reminder => _mapper.Map<Category>(reminder),
                SettingDomainModel setting => _mapper.Map<Setting>(setting),
                ToDoItemDomainModel item => _mapper.Map<ToDoItem>(item),
                ToDoListDomainModel list => _mapper.Map<ToDoList>(list),
                UserGroupDomainModel group => _mapper.Map<UserGroup>(group),

                _ => throw new ArgumentOutOfRangeException($"The Type {typeof(T).FullName} ist not recognized or not handled correctly!"),
            };
        }
    }
}
