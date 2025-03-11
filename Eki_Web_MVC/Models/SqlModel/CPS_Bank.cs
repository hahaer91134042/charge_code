using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DevLibs;
using Newtonsoft.Json;

namespace Eki_Web_MVC.CPS
{
    [DbTableSet("CPS_Bank")]
    public class CPS_Bank : BaseDbDAO,IConvertResponse<object>
    {
        [DbRowKey("Name", DbAction.Update)]
        public string Name { get; set; } = "";
        [DbRowKey("beEnable", DbAction.Update)]
        public bool beEnable { get; set; } = true;
        [DbRowKey("Salt", DbAction.Update)]
        public string Salt { get; set; } = "";
        [DbRowKey("Cipher",DbAction.Update)]
        public string Cipher { get; set; } = "";
        [DbRowKey("CommunityId")]
        public int CommunityId { get; set; }

        [JsonIgnore]
        public BankDecode decode
        {
            get
            {
                if (_bank == null)
                    _bank = EkiEncoder.AES.decode<BankDecode>(Cipher, Salt);
                return _bank;
            }
        }
        private BankDecode _bank;


        public override bool CreatById(int id) => EkiSQL.ekicps.loadDataById(id, this);

        public override int Insert(bool isReturnId = false) => EkiSQL.ekicps.insert(this, isReturnId);
        public override bool Update() => EkiSQL.ekicps.update(this);
        public override bool Delete() => EkiSQL.ekicps.delete(this);

        public object convertToResponse()
            => new
            {
                Name,
                decode.Serial,
                decode.Account,
                decode.Code,
                decode.Sub
            };

        public class BankDecode : IEncodeSet
        {
            public string Serial { get; set; } = "";
            public string Code { get; set; } = "";
            public string Account { get; set; } = "";
            public string Sub { get; set; } = "";

            public IHashCodeSet hashSet() => EkiHashCode.SHA1;
        }
    }
}