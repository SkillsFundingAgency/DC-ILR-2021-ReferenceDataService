﻿using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using ESFA.DC.ILR.ReferenceDataService.Service.Interface;
using ESFA.DC.Serialization.Interfaces;

namespace ESFA.DC.ILR.ReferenceDataService.Service
{
    public class ZipArchiveJsonFileService : IZipArchiveFileService
    {
        private readonly IJsonSerializationService _jsonSerializationService;

        public ZipArchiveJsonFileService(IJsonSerializationService jsonSerializationService)
        {
            _jsonSerializationService = jsonSerializationService;
        }

        public T RetrieveModel<T>(ZipArchive zipArchive, string fileName)
        {
            T model = (T)Activator.CreateInstance(typeof(T));

            using (var stream = zipArchive.GetEntry(fileName).Open())
            {
                model = _jsonSerializationService.Deserialize<T>(stream);
            }

            return model;
        }

        public IReadOnlyCollection<T> RetrieveModels<T>(ZipArchive zipArchive, string fileName, Func<T, bool> predicate = null)
        {
            using (var stream = zipArchive.GetEntry(fileName).Open())
            {
                var enumerable = _jsonSerializationService.DeserializeCollection<T>(stream);

                if (predicate != null)
                {
                    enumerable = enumerable.Where(predicate);
                }

                return enumerable.ToList();
            }
        }
    }
}
