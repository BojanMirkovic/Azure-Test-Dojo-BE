﻿using Domain_Layer.Models.Module;

namespace Infrastructure_Layer.Repositories.Module
{
    public interface IModuleRepository
    {
        Task DeleteModulesByCourseIdAsync(string courseId);
        Task<List<ModuleModel>> GetAllModulesByCourseId(string courseId);
        Task CreateModuleAsync(ModuleModel modul);
        Task<ModuleModel> GetModuleByIdAsync(string moduleId);
        Task<bool> UpdateModuleAsync(ModuleModel module);
    }
}
