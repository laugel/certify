﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Certify.Models.Config;

namespace Certify.Models.Providers
{
    public class DnsZone
    {
        public string ZoneId { get; set; }
        public string Name { get; set; }
    }

    public class DnsRecord
    {
        public string ZoneId { get; set; }
        public string RecordId { get; set; }

        public string TargetDomainName { get; set; }

        public string RecordType { get; set; } = "TXT";
        public string RootDomain { get; set; }

        public string RecordName { get; set; }
        public string RecordValue { get; set; }
    }

    public interface IDnsProvider
    {
        Task<bool> InitProvider();

        Task<ActionResult> Test();

        Task<ActionResult> CreateRecord(DnsRecord request);

        Task<ActionResult> DeleteRecord(DnsRecord request);

        Task<List<DnsZone>> GetZones();

        int PropagationDelaySeconds { get; }

        string ProviderId { get; }

        string ProviderTitle { get; }

        string ProviderDescription { get; }

        string ProviderHelpUrl { get; }

        List<ProviderParameter> ProviderParameters { get; }
    }

    public abstract class DnsProviderBase
    {
        /// <summary>
        /// Where a record name is in the form _acme-challenge.www.subdomain.domain.com, determine
        /// the root domain (i.e domain.com or subdomain.domain.com) info
        /// </summary>
        /// <param name="recordName"></param>
        /// <returns></returns>
        public async Task<DnsRecord> DetermineZoneDomainRoot(string recordName, string zoneId)
        {
            var zones = await GetZones();

            if (zoneId != null)
            {
                var zone = zones.FirstOrDefault(z => z.ZoneId.ToLower() == zoneId.ToLower().Trim());
                if (zone != null)
                {
                    return new DnsRecord
                    {
                        RootDomain = zone.Name,
                        ZoneId = zone.ZoneId
                    };
                }
            }

            // if we don't have a zone Id or the zone wasn't found, try to match domain to zone list

            var info = new DnsRecord { RecordType = "TXT" };

            foreach (var z in zones.OrderBy(zn => zn.Name.Length))
            {
                if (recordName.EndsWith(z.Name) && (info.RootDomain == null || z.Name.Length > info.RootDomain.Length))
                {
                    info.RootDomain = z.Name;
                    info.ZoneId = z.ZoneId;
                }
            }
            return info;
        }

        public string NormaliseRecordName(DnsRecord info, string recordName)
        {
            var result = recordName.Replace(info.RootDomain, "");
            result = result.TrimEnd('.');
            return result;
        }

        public abstract Task<List<DnsZone>> GetZones();
    }
}
