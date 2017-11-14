using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Autofac;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Internals.Fibers;

namespace CoreTesting_00.Controllers
{
    public static class BotBuilderStuff
    {
        public static void UpdateContainer()
        {
            Conversation.UpdateContainer(builder =>
            {
                builder
                    .RegisterType<Type_NetCoreSurrogate>()
                    .Keyed<Serialization.ISurrogateProvider>(FiberModule.Key_SurrogateProvider)
                    .SingleInstance();

                builder
                    .RegisterType<Delegate_NetCoreSurrogate>()
                            .Keyed<Serialization.ISurrogateProvider>(FiberModule.Key_SurrogateProvider)
                            .SingleInstance();

                builder
                    .RegisterType<MethodInfo_NetCoreSurrogate>()
                            .Keyed<Serialization.ISurrogateProvider>(FiberModule.Key_SurrogateProvider)
                            .SingleInstance();

            });
        }

        private const int Priority = 10;

        public sealed class Type_NetCoreSurrogate : Serialization.ISurrogateProvider
        {
            bool Serialization.ISurrogateProvider.Handles(Type type, StreamingContext context, out int priority)
            {
                bool handles = typeof(Type).IsAssignableFrom(type);
                priority = handles ? Priority : 0;
                return handles;
            }

            void ISerializationSurrogate.GetObjectData(object obj, SerializationInfo info, StreamingContext context)
            {
                var instance = (Type)obj;
                info.AddValue(typeof(Type).Name, instance.FullName);
            }

            object ISerializationSurrogate.SetObjectData(object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector)
            {
                return Type.GetType((string)info.GetValue(typeof(Type).Name, typeof(string)));
            }
        }

        public sealed class Delegate_NetCoreSurrogate : Serialization.ISurrogateProvider
        {
            bool Serialization.ISurrogateProvider.Handles(Type type, StreamingContext context, out int priority)
            {
                bool handles = typeof(Delegate).IsAssignableFrom(type);
                priority = handles ? Priority : 0;
                return handles;
            }

            void ISerializationSurrogate.GetObjectData(object obj, SerializationInfo info, StreamingContext context)
            {
                var instance = (Delegate)obj;
                info.AddValue("type", instance.GetType());
                info.AddValue("target", instance.Target);
                info.AddValue("method", instance.Method);
            }

            object ISerializationSurrogate.SetObjectData(object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector)
            {
                var type = (Type)info.GetValue("type", typeof(Type));
                var target = info.GetValue("target", typeof(object));
                var method = info.GetValue("method", typeof(MethodInfo));
                throw new NotImplementedException();
            }
        }

        public sealed class MethodInfo_NetCoreSurrogate : Serialization.ISurrogateProvider
        {
            bool Serialization.ISurrogateProvider.Handles(Type type, StreamingContext context, out int priority)
            {
                bool handles = typeof(MethodInfo).IsAssignableFrom(type);
                priority = handles ? Priority : 0;
                return handles;
            }

            void ISerializationSurrogate.GetObjectData(object obj, SerializationInfo info, StreamingContext context)
            {
                var instance = (MethodInfo)obj;
            }

            object ISerializationSurrogate.SetObjectData(object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector)
            {
                throw new NotImplementedException();
            }
        }

    }
}
