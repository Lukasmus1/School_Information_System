﻿using VUTIS2.BL.Models;
using VUTIS2.DAL.Entities;

namespace VUTIS2.BL.Facades;

public interface IActivityFacade : IFacade<ActivityEntity, ActivityListModel, ActivityDetailModel>
{
    Task<IEnumerable<ActivityListModel>> GetAsyncFromSubject(Guid Id);
    IEnumerable<ActivityListModel> GetOrderedByStartTimeAsc(IEnumerable<ActivityListModel> activities);
    IEnumerable<ActivityListModel> GetOrderedByStartTimeDesc(IEnumerable<ActivityListModel> activities);
    IEnumerable<ActivityListModel> GetOrderedByEndTimeAsc(IEnumerable<ActivityListModel> activities);
    IEnumerable<ActivityListModel> GetOrderedByEndTimeDesc(IEnumerable<ActivityListModel> activities);

    public Task<IEnumerable<ActivityListModel>> GetAsyncBySubject(Guid subjectId);
}
