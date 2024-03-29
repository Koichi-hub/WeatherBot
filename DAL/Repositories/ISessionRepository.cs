﻿using Core.Entities;

namespace DAL.Repositories
{
    public interface ISessionRepository
    {
        Task<Session?> GetAsync(long userId);

        Task<Session> CreateAsync(Session model);

        Task<Session> UpdateAsync(Session session);

        Task<bool> CanGuestGetWeather(Session session);
    }
}
