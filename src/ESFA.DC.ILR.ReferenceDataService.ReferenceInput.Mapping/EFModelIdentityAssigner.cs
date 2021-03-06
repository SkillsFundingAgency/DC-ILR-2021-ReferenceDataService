﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ESFA.DC.ILR.ReferenceDataService.ReferenceInput.Mapping.Interface;

namespace ESFA.DC.ILR.ReferenceDataService.ReferenceInput.Mapping
{
    public class EFModelIdentityAssigner : IEFModelIdentityAssigner
    {
        public void AssignIdsByType<T>(IEnumerable<T> source)
        {
            GenerateIds(source);
        }

        private static bool GenerateIds<T>(T model)
        {
            if (model == null)
            {
                return false;
            }

            var ids = new Dictionary<string, int>();
            RecurseAndProcessChildProperties(model, ids, null);

            return true;
        }

        private static bool GenerateIds<T>(IEnumerable<T> models)
        {
            if (models == null)
            {
                return false;
            }

            if (!models.Any())
            {
                return true;
            }

            var ids = new Dictionary<string, int>();
            foreach (var item in models)
            {
                RecurseAndProcessChildProperties(item, ids, null);
            }

            return true;
        }

        private static void RecurseAndProcessChildProperties(object model, Dictionary<string, int> idsToAssign, int? idToAssignSupplied, string parentObjectName = null, int parentIdValue = 0)
        {
            var intType = typeof(int);
            Type objType = model.GetType();
            var properties = objType.GetProperties().ToList();

            //Set the Id of this entity
            var idToAssign = idToAssignSupplied ?? GetAndIncrementIdForType(idsToAssign, objType.Name);
            SetIntProperty(model, properties, "Id", idToAssign);

            // Set the parent Id if relevant
            if (!string.IsNullOrEmpty(parentObjectName) && parentIdValue != 0)
            {
                SetIntProperty(model, properties, $"{parentObjectName}_Id", parentIdValue);
            }

            // Deal with any children in collections
            var listsOfCollectionChildEntitys = properties.Where(p => p.PropertyType.IsGenericType && p.PropertyType.GetGenericTypeDefinition() == typeof(ICollection<>)).ToList();

            foreach (var childEntityList in listsOfCollectionChildEntitys)
            {
                object propValue = childEntityList.GetValue(model, null);
                var elems = propValue as IEnumerable;
                if (elems != null)
                {
                    var elemsEnumerator = elems.GetEnumerator();
                    if (elemsEnumerator.MoveNext())
                    {
                        var item = elemsEnumerator.Current;
                        var typeName = item.GetType().Name;
                        var id = GetIdForType(idsToAssign, typeName);

                        do
                        {
                            item = elemsEnumerator.Current;
                            RecurseAndProcessChildProperties(item, idsToAssign, id++, objType.Name, idToAssign);
                        } while (elemsEnumerator.MoveNext());

                        UpdateIdForType(idsToAssign, typeName, id);
                    }
                }
            }

            // Deal with any children not in a collection
            var instanceOfChildEntities = properties.Where(p => p.PropertyType.IsClass && p.PropertyType.Assembly.FullName.Contains("ESFA.DC.ILR.ReferenceDataService.ReferenceInput.Model")).ToList();

            foreach (var instanceOfChildEntity in instanceOfChildEntities)
            {
                object childValue = instanceOfChildEntity.GetValue(model, null);

                if (childValue != null)
                {
                    RecurseAndProcessChildProperties(childValue, idsToAssign, null);
                }
            }
        }

        private static int GetIdForType(Dictionary<string, int> idsToAssign, string name)
        {
            if (!idsToAssign.TryGetValue(name, out int id))
            {
                // Add this key to the dictionary, add 1 for next retrieval, and return 1 for this retrieval
                idsToAssign.Add(name, 1);
                return 1;
            }

            return id;
        }

        private static int GetAndIncrementIdForType(Dictionary<string, int> idsToAssign, string name)
        {
            if (!idsToAssign.TryGetValue(name, out int id))
            {
                // Add this key to the dictionary, add 2 for next retrieval, and return 1 for this retrieval
                idsToAssign.Add(name, 2);
                return 1;
            }

            idsToAssign[name] = id + 1;
            return id;
        }

        private static void UpdateIdForType(Dictionary<string, int> idsToAssign, string name, int newValue)
        {
            if (!idsToAssign.TryGetValue(name, out int id))
            {
                throw new ApplicationException($"Id Type {name} does not exists to update");
            }

            if (id >= newValue)
            {
                throw new ApplicationException($"Id Type {newValue} is invalid update for {id}");
            }

            idsToAssign[name] = newValue;
        }

        private static void SetIntProperty(object entity, List<PropertyInfo> properties, string idName, int idToAssign)
        {
            var idProperty = properties.FirstOrDefault(p => (p.PropertyType == typeof(int) || p.PropertyType == typeof(int?)) && p.Name == idName);

            if (idProperty == null)
            {
                throw new ApplicationException($"Unable to find a property called {idName} to set.");
            }

            idProperty.SetValue(entity, idToAssign);
        }

    }
}
