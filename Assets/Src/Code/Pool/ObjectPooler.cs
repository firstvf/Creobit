﻿using System.Collections.Generic;
using UnityEngine;

namespace Assets.Src.Code.Pool
{
    public class ObjectPooler<T> where T : MonoBehaviour
    {
        private readonly List<T> _poolList;
        private readonly Transform _container;
        private readonly T _prefab;

        public ObjectPooler(T prefab, Transform container, int count)
        {
            _prefab = prefab;
            _container = container;
            _poolList = new List<T>();

            CreatePool(count);
        }

        public List<T> GetList() => _poolList;

        public T GetFreeObjectFromPool()
        {
            if (HasFreeObject(out T prefab))
                return prefab;
            else return CreateObject(true);

            throw new System.Exception("There is no objects in pool");
        }

        private void CreatePool(int count)
        {
            for (int i = 0; i < count; i++)
                CreateObject();
        }

        private T CreateObject(bool isActiveByDefault = false)
        {
            var prefab = Object.Instantiate(_prefab, _container);
            prefab.gameObject.SetActive(isActiveByDefault);
            _poolList.Add(prefab);

            return prefab;
        }

        private bool HasFreeObject(out T prefab)
        {
            foreach (var obj in _poolList)
                if (!obj.gameObject.activeInHierarchy)
                {
                    prefab = obj;
                    prefab.gameObject.SetActive(true);
                    return true;
                }
            prefab = null;

            return false;
        }
    }
}