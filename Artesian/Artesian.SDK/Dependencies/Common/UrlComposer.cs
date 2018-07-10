﻿using Artesian.SDK.Dependencies;
using Artesian.SDK.Dependencies.TimeTools;
using NodaTime;
using NodaTime.Text;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace Artesian.SDK.Dependencies.Common
{
    public class UrlComposer
    {
        private string _baseUrl = "";
        private List<(string key, string value)> _tuples = new List<(string key, string value)>();
        private static LocalDatePattern _localDatePattern = LocalDatePattern.Iso;
        private static LocalDateTimePattern _localDateTimePattern = LocalDateTimePattern.GeneralIso;

        public UrlComposer(string baseUrl)
        {
            _baseUrl = baseUrl;
        }

        public UrlComposer AddQueryParam(string key, int? value)
        {
            if (value != null)
                _tuples.Add((key, $"{value.Value}"));
            return this;
        }

        public UrlComposer AddQueryParam(string key, string value)
        {
            if (!string.IsNullOrWhiteSpace(value))
                _tuples.Add((key, value));
            return this;
        }

        public UrlComposer AddQueryParam(string key, bool value)
        {
            _tuples.Add((key, value ? "true" : "false"));
            return this;
        }

        public UrlComposer AddQueryParam(string key, LocalDate? value)
        {
            if (value != null)
                _tuples.Add((key, _localDatePattern.Format(value.Value)));
            return this;
        }


        public UrlComposer AddQueryParam(string key, LocalDateTime? value)
        {
            if (value != null)
                _tuples.Add((key, _localDateTimePattern.Format(value.Value)));
            return this;
        }

        public UrlComposer AddQueryParam<T>(string key, IEnumerable<T> values)
        {
            var toAdd = values?
                   .Where(w => !string.IsNullOrWhiteSpace($"{w}"))
                   .Select(s => (key, s.ToString()));

            if (toAdd != null && toAdd.Any())
                this._tuples.AddRange(toAdd);

            return this;
        }

        public UrlComposer AddQueryParam((string key, string value)[] query)
        {
            this._tuples.AddRange(
                query
                .Where(w => !string.IsNullOrWhiteSpace(w.value))
                .Select(s => (s.key, s.value))
            );
            return this;
        }

        public override string ToString()
        {
            return $"{_baseUrl}{_queryParams(_tuples)}";
        }

        public static implicit operator string(UrlComposer url) { return url.ToString(); }

        private static string _queryParams(List<(string key, string value)> query)
        {
            if (query.Any())
                return $"?{string.Join("&", query.Select(v => $"{v.key}={WebUtility.UrlEncode(v.value)}"))}";
            return "";
        }

        public static string ToUrlParam(LocalDateRange range)
        {
            return $"{_localDatePattern.Format(range.Start)}/{_localDatePattern.Format(range.End)}";
        }

        public static string ToUrlParam(LocalDate date)
        {
            return _localDatePattern.Format(date);
        }

        public static string ToUrlParam(LocalDateTimeRange range)
        {
            return $"{_localDateTimePattern.Format(range.Start)}/{_localDateTimePattern.Format(range.End)}";
        }

        public static string ToUrlParam(LocalDateTime dateTime)
        {
            return _localDateTimePattern.Format(dateTime);
        }
    }
}
