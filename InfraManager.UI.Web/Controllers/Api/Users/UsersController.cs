using InfraManager.BLL.Users;
using InfraManager.CrossPlatform.WebApi.Contracts.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;
using System.Threading.Tasks;
using Inframanager.BLL;
using InfraManager.BLL.ServiceDesk;

namespace InfraManager.UI.Web.Controllers.Api.Users
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly IUserBLL _service;
        private readonly IEmployeeWorkloadBLL _employeeWorkloadBLL;

        public UsersController(IUserBLL service, IEmployeeWorkloadBLL employeeWorkloadBLL)
        {
            _service = service;
            _employeeWorkloadBLL = employeeWorkloadBLL;
        }

        [HttpGet("{id}")]
        public Task<UserDetailsModel> FindAsync(Guid id, CancellationToken cancellationToken = default)
            => _service.DetailsAsync(id, cancellationToken);

        [HttpGet("by-email/{email}")]
        public Task<UserListItem[]> FindByEmailAsync(string email, CancellationToken cancellationToken = default) //TODO Remove and move to filter
            => _service.FindByEmailAsync(email, cancellationToken);

        [HttpGet("by-login/{login}")]
        public Task<UserDetailsModel> FindByLoginAsync(string login, CancellationToken cancellationToken = default)
            => _service.DetailsByLoginAsync(login, cancellationToken);
        
        [HttpGet("single-by-email/{email}")]
        public Task<UserDetailsModel> FindSingleByEmailAsync(string email, CancellationToken cancellationToken = default)
            => _service.DetailsByEmailAsync(email, cancellationToken);

        [HttpGet("userMails")]
        public async Task<UserEmailDetails[]> GetUserEmailsAsync(BaseFilter filter, CancellationToken cancellationToken = default)
            => await _service.GetEmailsAsync(filter, cancellationToken);

        [HttpGet]
        public Task<UserListItem[]> ListAsync([FromQuery] UserListFilter filter, CancellationToken cancellationToken = default)
            => _service.ListAsync(filter, cancellationToken);


        [HttpPost("ListDetailsAsync")]
        public Task<UserDetailsModel[]> ListDetailsAsync([FromBody] UserListFilter filter, CancellationToken cancellationToken)
            => _service.ListDetailsAsync(filter, cancellationToken);

        /// <summary>
        /// Метод фильтрует пользователся по BaseFilter
        /// (Опционально) фильтрует по subdivision или organization
        /// </summary>
        /// <param name="filter">Параметры для фильтрации пользователей</param>
        /// <param name="cancellationToken"></param>
        [HttpGet("Table")]
        public async Task<UserDetailsModel[]> GetTableAsync([FromQuery] UserFilter filter, CancellationToken cancellationToken) 
            => await _service.GetTableAsync(filter, cancellationToken);

        /// <summary>
        /// Метод обновляет данные о пользователе в базе данных
        /// </summary>
        [HttpPut("{id}")]
        public async Task UpdateAsync([FromRoute] Guid id, [FromBody] UserData userData, CancellationToken cancellationToken)
            => await _service.UpdateAsync(id, userData, cancellationToken);

        /// <summary>
        /// Метод создает нового пользователя в базе данных
        /// </summary>
        [HttpPost]
        public async Task CreateAsync([FromBody] UserData userData, CancellationToken cancellationToken) 
            => await _service.CreateAsync(userData, cancellationToken);

        /// <summary>
        /// Метод удаляет пользователя из базы данных 
        /// </summary>
        /// <param name="id">IMObjID</param>
        /// <param name="cancellationToken"></param>
        [HttpDelete("{id}")]
        public async Task DeleteAsync([FromRoute] Guid id, CancellationToken cancellationToken)
            => await _service.DeleteAsync(id, cancellationToken);
        

        [HttpPost("reports/workload")]
        public async Task<EmployeeWorkloadListItem[]> GetEmployeeWorkloadReportAsync(
            [FromBody] WorkloadListData data,
            [FromQuery] ClientPageFilter pageBy,
            CancellationToken cancellationToken = default)
            => await _employeeWorkloadBLL.GetEmployeeWorkloadReportAsync(data, pageBy, cancellationToken);
        
    }
}
