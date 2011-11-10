using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using eclectica.co.uk.Service.Abstract;
using eclectica.co.uk.Domain.Abstract;
using eclectica.co.uk.Domain.Entities;
using mab.lib.SimpleMapper;
using eclectica.co.uk.Service.Entities;
using System.Data.Objects;

namespace eclectica.co.uk.Service.Concrete
{
    public class RedirectServices : IRedirectServices
    {
        private IRedirectRepository _redirectRepository;

        public RedirectServices(IRedirectRepository redirectRepository)
        {
            _redirectRepository = redirectRepository;
        }

        public IEnumerable<RedirectModel> All()
        {
            return Mapper.MapList<Redirect, RedirectModel>(_redirectRepository.All().ToList());
        }

        public RedirectModel GetRedirect(int id)
        {
            return Mapper.Map<Redirect, RedirectModel>(_redirectRepository.Get(id));
        }

        public void AddRedirect(RedirectModel model)
        {
            var redirect = new Redirect();

            Mapper.CopyProperties<RedirectModel, Redirect>(model, redirect);

            _redirectRepository.Add(redirect);

            model.RedirectID = redirect.RedirectID;
        }

        public void UpdateRedirect(RedirectModel model)
        {
            var redirect = _redirectRepository.Get(model.RedirectID);

            Mapper.CopyProperties<RedirectModel, Redirect>(model, redirect);

            _redirectRepository.Update(redirect);
        }

        public void DeleteRedirect(int id)
        {
            _redirectRepository.Remove(id);
        }
    }
}
