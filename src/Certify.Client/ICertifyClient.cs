﻿using Certify.Models;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Certify.Client
{
    /// <summary>
    /// Client to talk to the core Certify Service 
    /// </summary>
    public interface ICertifyClient
    {
        /*
         *
        Preferences: /preferences

        Settings to be save/loaded via the client api (still tied to Core App)

        GetPreferences()
        SetPreferences()

        Primary Contact Registration /contact/

        GetPrimaryContact
        SetPrimaryContact : set the current contact registration for all subsequent requests

        Web Server Status: /server

        GetServerSummary("IIS") - summary of general info for this server, IIS Version, .Net version etc
        IsServerAvailable("IIS");
        GetServerVersion("IIS")
        GetServerSiteList("IIS")
        GetServerSiteDomains("IIS",siteId);

        Managed Sites: /managedsites/

        GetManagedSites(filter)
        GetManagedSite(id)
        AddOrUpdateManagedSite
        DeleteManagedItem

        PreviewAutoRenewal - return list of managed sites which would be currently included in an auto renew run
        BeginAutoRenewal - Begin auto renewal process and returns list of managedsites included in this run
        BeginCertificateRequest(managedsite id) - Begins a single manage site certificate request
        CheckCertificateRequest(managedsite id) - poll until completed/failed or timeout
        */

        #region System

        Task<string> GetAppVersion();

        Task<UpdateCheck> CheckForUpdates();

        #endregion System

        #region Server

        Task<bool> IsServerAvailable(StandardServerTypes serverType);

        Task<List<SiteBindingItem>> GetServerSiteList(StandardServerTypes serverType);

        Task<System.Version> GetServerVersion(StandardServerTypes serverType);

        #endregion Server

        #region Preferences

        Task<Preferences> GetPreferences();

        Task<bool> SetPreferences(Preferences preferences);

        #endregion Preferences

        #region Managed Sites

        Task<List<ManagedSite>> GetManagedSites(ManagedSiteFilter filter);

        Task<List<ManagedSite>> GetManagedSite(string managedSiteId);

        Task<ManagedSite> UpdateManagedSite(ManagedSite site);

        Task<bool> DeleteManagedSite(string managedSiteId);

        Task<APIResult> RevokeManageSiteCertificate(string managedSiteId);

        Task<List<ManagedSite>> BeginAutoRenewal();

        Task BeginCertificateRequest(string managedSiteId);

        Task<string> CheckCertificateRequest(string managedSiteId);

        Task<List<DomainOption>> GetServerSiteDomains(string mamagedSiteId);

        Task<APIResult> TestChallengeConfiguration(ManagedSite site);

        #endregion Managed Sites

        #region Contacts

        Task<string> GetPrimaryContact();

        Task<bool> SetPrimaryContact(ContactRegistration contact);

        #endregion Contacts
    }
}