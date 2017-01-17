using System;
using System.Collections.Generic;
using System.Linq;

namespace DocumentsExchange.Hub.Services
{
    public class ConnectionInfo
    {
        public ConnectionInfo(string connectionId)
        {
            ConnectionId = connectionId;
            LastActivity = DateTimeOffset.UtcNow;
        }

        public DateTimeOffset LastActivity { get; set; }

        public string ConnectionId { get; private set; }
    }

    public class MappingInfo<TUser> where TUser : class
    {
        public MappingInfo(TUser user)
        {
            User = user;
            Connections = new HashSet<ConnectionInfo>();
        }

        public TUser User { get; private set; }

        public HashSet<ConnectionInfo> Connections { get; private set; }
    }

    public class ConnectionPool<TUser, TKey> where TUser : class
    {
        private readonly Func<TUser, TKey> _keySelector;
        private readonly object _mappingLock = new object();

        private readonly Dictionary<TKey, MappingInfo<TUser>> _connections =
            new Dictionary<TKey, MappingInfo<TUser>>();

        public ConnectionPool(Func<TUser, TKey> keySelector)
        {
            _keySelector = keySelector;
        }

        public int Count
        {
            get
            {
                return _connections.Count;
            }
        }

        public void TryAdd(TUser user, string connectionId)
        {
            lock (_mappingLock)
            {
                MappingInfo<TUser> mappingInfo;
                TKey key = _keySelector(user);

                if (!_connections.TryGetValue(key, out mappingInfo))
                {
                    mappingInfo = new MappingInfo<TUser>(user);
                    _connections.Add(key, mappingInfo);
                }

                if (mappingInfo.Connections.FirstOrDefault(x => x.ConnectionId == connectionId) == null)
                    mappingInfo.Connections.Add(new ConnectionInfo(connectionId));
            }
        }

        public IEnumerable<string> GetConnections(TUser user)
        {
            lock (_mappingLock)
            {
                MappingInfo<TUser> mappingInfo;
                TKey key = _keySelector(user);

                if (_connections.TryGetValue(key, out mappingInfo))
                {
                    return mappingInfo.Connections.Select(c => c.ConnectionId);
                }
            }

            return Enumerable.Empty<string>();
        }

        public int GetUsersCount(Func<TUser, bool> predicate)
        {
            lock (_mappingLock)
            {
                return _connections.Values.Select(c => c.User).Count(predicate);
            }
        }

        public int GetOnlineUsersCount(Func<TUser, bool> predicate)
        {
            lock (_mappingLock)
            {
                return _connections.Values.Where(x => x.Connections.Count > 0).Select(c => c.User).Count(predicate);
            }
        }

        public IEnumerable<TUser> GetUsers(Func<TUser, bool> predicate)
        {
            lock (_mappingLock)
            {
                return _connections.Values.Select(c => c.User).Where(predicate);
            }
        }

        public IDictionary<TKey, List<ConnectionInfo>> GetConnections()
        {
            lock (_mappingLock)
            {
                return _connections.Values.Select(c => new
                {
                    Key = _keySelector(c.User),
                    Values = c.Connections.ToList()
                })
                        .ToDictionary(x => x.Key, x => x.Values);
            }
        }

        public TUser TryGetUser(Func<TUser, bool> predicate)
        {
            lock (_mappingLock)
            {
                return _connections.Values.Select(c => c.User).FirstOrDefault(predicate);
            }
        }

        public void Remove(TUser user, string connectionId)
        {
            lock (_mappingLock)
            {
                MappingInfo<TUser> mappingInfo;
                TKey key = _keySelector(user);

                if (!_connections.TryGetValue(key, out mappingInfo))
                {
                    return;
                }

                var connection = mappingInfo.Connections.FirstOrDefault(c => c.ConnectionId == connectionId);
                if (connection != null)
                    mappingInfo.Connections.Remove(connection);

                if (mappingInfo.Connections.Count == 0)
                {
                    _connections.Remove(key);
                }
            }
        }

        public void Remove(TUser user)
        {
            lock (_mappingLock)
            {
                MappingInfo<TUser> mappingInfo;
                TKey key = _keySelector(user);

                if (!_connections.TryGetValue(key, out mappingInfo))
                {
                    return;
                }

                _connections.Remove(key);
            }
        }

        public MappingInfo<TUser> Remove(TKey key, string connectionId)
        {
            lock (_mappingLock)
            {
                MappingInfo<TUser> mappingInfo;

                if (!_connections.TryGetValue(key, out mappingInfo))
                {
                    return null;
                }

                var connection = mappingInfo.Connections.FirstOrDefault(c => c.ConnectionId == connectionId);
                if (connection != null)
                    mappingInfo.Connections.Remove(connection);

                if (mappingInfo.Connections.Count == 0)
                {
                    _connections.Remove(key);
                }

                return mappingInfo;
            }
        }
    }
}
