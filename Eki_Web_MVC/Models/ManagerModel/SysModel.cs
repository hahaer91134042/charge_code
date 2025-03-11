using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DevLibs;

namespace Eki_Web_MVC
{
    public class SysModel:IBaseManagerModel
    {
        public SysManager user;
        public SysModel(SysManager u)
        {
            user = u;
        }

        public bool AddUser(UserReq user)
        {
            var newUser = new SysManager()
            {
                Account=user.acc,
                Pwd=SysManager.creatPwdCipher(user.pwd),
                Name=user.name,
                Email=user.email,
                Phone=user.phone,
                Ip=WebUtil.clientIP()
            };

            var group = (from g in EkiSQL.ekicps.table<SysGroup>()
                         where g.beEnable
                         where g.Lv == user.lv
                         select g).FirstOrDefault();

            newUser.GroupId = group == null ? 0 : group.Id;

            var mID = newUser.Insert(true);

            return mID > 0;
        }

        public bool editUser(UserReq user)
        {
            
            var editUser = new SysManager().Also(m => m.CreatById(user.id));
            editUser.Name = user.name;
            editUser.Pwd = SysManager.creatPwdCipher(user.pwd);
            editUser.Phone = user.phone;
            editUser.Email = user.email;
            editUser.beEnable = user.beEnable;

            var group = (from g in EkiSQL.ekicps.table<SysGroup>()
                         where g.Lv == user.lv
                         select g).FirstOrDefault();

            editUser.GroupId = group == null ? 0 : group.Id;


            return editUser.Update();
        }

        public void addNewMenu(MenuReq req)
        {
            var menu = new SysMenu()
            {
                ParentId=req.parentId,
                Name=req.name,
                GroupLv=req.groupLv,
                Path=req.path,
                Type=req.type,
                Sort=req.sort,
                beEnable=req.beEnable                
            };

            menu.Insert();
        }

        public void delMenu(MenuReq req)
        {
            var menu = new SysMenu().Also(m => m.CreatById(req.id));
            menu.Delete();
        }

        public void editMenu(MenuReq req)
        {
            var menu = new SysMenu().Also(m => m.CreatById(req.id));
            menu.Name = req.name;
            menu.ParentId = req.parentId;
            menu.Path = req.path;
            menu.Type = req.type;
            menu.GroupLv = req.groupLv;
            menu.beEnable = req.beEnable;

            if (!menu.Update())
                throw new ArgumentException();
        }
    }
}