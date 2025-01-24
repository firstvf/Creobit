using System;

namespace Assets.Src.Code.Data
{
    public interface IDataService
    {
        public void Save(string key, object data, Action<bool> callback = null);
        public void Load<T>(string key, Action<T> callback);
    }
}