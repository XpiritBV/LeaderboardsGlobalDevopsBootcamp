using System;
using System.Collections.Generic;
using System.Text;

namespace gdbcLeaderBoard.Domain.VSTSModels
{
    public class WorkItemUpdate
    {
        public string subscriptionId { get; set; }
        public int notificationId { get; set; }
        public string id { get; set; }
        public string eventType { get; set; }
        public string publisherId { get; set; }
        public object message { get; set; }
        public object detailedMessage { get; set; }
        public Resource resource { get; set; }
        public string resourceVersion { get; set; }
        public Resourcecontainers resourceContainers { get; set; }
        public DateTime createdDate { get; set; }
    }

    public class Resource
    {
        public int id { get; set; }
        public int workItemId { get; set; }
        public int rev { get; set; }
        public Revisedby revisedBy { get; set; }
        public DateTime revisedDate { get; set; }
        public Fields fields { get; set; }
        public _Links1 _links { get; set; }
        public string url { get; set; }
        public Revision revision { get; set; }
    }

    public class Revisedby
    {
        public string id { get; set; }
        public string name { get; set; }
        public string displayName { get; set; }
        public string url { get; set; }
        public _Links _links { get; set; }
        public string uniqueName { get; set; }
        public string imageUrl { get; set; }
        public string descriptor { get; set; }
    }

    public class _Links
    {
        public Avatar avatar { get; set; }
    }

    public class Avatar
    {
        public string href { get; set; }
    }

    public class Fields
    {
        public SystemRev SystemRev { get; set; }
        public SystemAuthorizeddate SystemAuthorizedDate { get; set; }
        public SystemReviseddate SystemRevisedDate { get; set; }
        public SystemState SystemState { get; set; }
        public SystemReason SystemReason { get; set; }
        public SystemChangeddate SystemChangedDate { get; set; }
        public SystemWatermark SystemWatermark { get; set; }
        public SystemBoardcolumn SystemBoardColumn { get; set; }
        public MicrosoftVSTSCommonCloseddate MicrosoftVSTSCommonClosedDate { get; set; }
    }

    public class SystemRev
    {
        public int oldValue { get; set; }
        public int newValue { get; set; }
    }

    public class SystemAuthorizeddate
    {
        public DateTime oldValue { get; set; }
        public DateTime newValue { get; set; }
    }

    public class SystemReviseddate
    {
        public DateTime oldValue { get; set; }
        public DateTime newValue { get; set; }
    }

    public class SystemState
    {
        public string oldValue { get; set; }
        public string newValue { get; set; }
    }

    public class SystemReason
    {
        public string oldValue { get; set; }
        public string newValue { get; set; }
    }

    public class SystemChangeddate
    {
        public DateTime oldValue { get; set; }
        public DateTime newValue { get; set; }
    }

    public class SystemWatermark
    {
        public int oldValue { get; set; }
        public int newValue { get; set; }
    }

    public class SystemBoardcolumn
    {
        public string oldValue { get; set; }
        public string newValue { get; set; }
    }

    public class MicrosoftVSTSCommonCloseddate
    {
        public DateTime oldValue { get; set; }
    }
    
    public class _Links1
    {
        public Self self { get; set; }
        public Workitemupdates workItemUpdates { get; set; }
        public Parent parent { get; set; }
        public Html html { get; set; }
    }

    public class Self
    {
        public string href { get; set; }
    }

    public class Workitemupdates
    {
        public string href { get; set; }
    }

    public class Parent
    {
        public string href { get; set; }
    }

    public class Html
    {
        public string href { get; set; }
    }

    public class Revision
    {
        public int id { get; set; }
        public int rev { get; set; }
        public Fields1 fields { get; set; }
        public _Links2 _links { get; set; }
        public string url { get; set; }
    }

    public class Fields1
    {
        public string SystemAreaPath { get; set; }
        public string SystemTeamProject { get; set; }
        public string SystemIterationPath { get; set; }
        public string SystemWorkItemType { get; set; }
        public string SystemState { get; set; }
        public string SystemReason { get; set; }
        public DateTime SystemCreatedDate { get; set; }
        public SystemCreatedby SystemCreatedBy { get; set; }
        public DateTime SystemChangedDate { get; set; }
        public SystemChangedby SystemChangedBy { get; set; }
        public string SystemTitle { get; set; }
        public string SystemBoardColumn { get; set; }
        public bool SystemBoardColumnDone { get; set; }
        public int MicrosoftVSTSCommonPriority { get; set; }
        public string MicrosoftVSTSCommonValueArea { get; set; }
        public float MicrosoftVSTSSchedulingEffort { get; set; }
        public string SystemDescription { get; set; }
        public string MicrosoftVSTSCommonAcceptanceCriteria { get; set; }
        public string SystemTags { get; set; }
    }

    public class SystemCreatedby
    {
        public string name { get; set; }
        public string displayName { get; set; }
        public string uniqueName { get; set; }
    }

    public class SystemChangedby
    {
        public string name { get; set; }
        public string displayName { get; set; }
        public string uniqueName { get; set; }
    }

    public class _Links2
    {
        public Self1 self { get; set; }
        public Workitemrevisions workItemRevisions { get; set; }
        public Parent1 parent { get; set; }
    }

    public class Self1
    {
        public string href { get; set; }
    }

    public class Workitemrevisions
    {
        public string href { get; set; }
    }

    public class Parent1
    {
        public string href { get; set; }
    }

    public class Resourcecontainers
    {
        public Collection collection { get; set; }
        public Account account { get; set; }
        public Project project { get; set; }
    }

    public class Collection
    {
        public string id { get; set; }
        public string baseUrl { get; set; }
    }

    public class Account
    {
        public string id { get; set; }
        public string baseUrl { get; set; }
    }

    public class Project
    {
        public string id { get; set; }
        public string baseUrl { get; set; }
    }
}
