﻿using System;
using System.Threading.Tasks;
using System.Xml;

namespace Microsoft.ServiceModel.Syndication
{
    public class XmlReaderWrapper : XmlReader
    {
        private XmlReader reader;

        private Func<Task<string>> getValueFunc;
        private Func<Task<string>> readElementStringFunc;
        private Func<Task<string>> readStringFunc;
        private Func<Task> readEndElementFunc;
        private Func<Task<XmlNodeType>> moveToContentFunc;
        private Func<Task> readStartElementFunc;
        private Func<string, string, Task> readStartElementFunc2;
        private Func<Task<bool>> isStartElementFunc;
        private Func<string, string, Task<bool>> isStartElementFunc2;
        private Func<Task> skipFunc;
        private Func<Task<bool>> readFunc;

        public XmlReaderWrapper(XmlReader reader)
        {
            if (reader == null)
            {
                throw new ArgumentNullException("reader");
            }

            this.reader = reader;

            if (this.reader.Settings.Async)
            {
                InitAsync();
            }
            else
            {
                Init();
            }
        }

        private void InitAsync()
        {
            this.getValueFunc = new Func<Task<string>>(async () => { return await this.reader.GetValueAsync(); });
            this.readElementStringFunc = new Func<Task<string>>(async () => { return await ReadElementStringAsync(this.reader); });
            this.readStringFunc = new Func<Task<string>>(async () => { return await ReadStringAsync(this.reader); });
            this.readEndElementFunc = new Func<Task>(async () => { await ReadEndElementAsync(this.reader); });
            this.moveToContentFunc = new Func<Task<XmlNodeType>>(async () => { return await this.reader.MoveToContentAsync(); });
            this.readStartElementFunc = new Func<Task>(async () => { await ReadStartElementAsync(this.reader); });
            this.readStartElementFunc2 = new Func<string, string, Task>(async (localname, ns) => { await ReadStartElementAsync(this.reader, localname, ns); });
            this.isStartElementFunc = new Func<Task<bool>>(async () => { return await IsStartElementAsync(this.reader); });
            this.isStartElementFunc2 = new Func<string, string, Task<bool>>(async (localname, ns) => { return await IsStartElementAsync(this.reader, localname, ns); });
            this.skipFunc = new Func<Task>(async () => { await this.reader.SkipAsync(); });
            this.readFunc = new Func<Task<bool>>(async () => { return await this.reader.ReadAsync(); });
        }

        private void Init()
        {
            this.getValueFunc = new Func<Task<string>>(() => { return Task.FromResult<string>(this.reader.Value); });
            this.readElementStringFunc = new Func<Task<string>>(() => { return Task.FromResult<string>(this.reader.ReadElementString()); });
            this.readStringFunc = new Func<Task<string>>(() => { return Task.FromResult<string>(this.reader.ReadString()); });
            this.readEndElementFunc = new Func<Task>(() => { this.reader.ReadEndElement(); return Task.CompletedTask; });
            this.moveToContentFunc = new Func<Task<XmlNodeType>>(() => { return Task.FromResult<XmlNodeType>(this.reader.MoveToContent()); });
            this.readStartElementFunc = new Func<Task>(() => { this.reader.ReadStartElement(); return Task.CompletedTask; });
            this.readStartElementFunc2 = new Func<string, string, Task>((localname, ns) => { this.reader.ReadStartElement(localname, ns); return Task.CompletedTask; });
            this.isStartElementFunc = new Func<Task<bool>>(() => { return Task.FromResult<bool>(this.reader.IsStartElement()); });
            this.isStartElementFunc2 = new Func<string, string, Task<bool>>((localname, ns) => { return Task.FromResult<bool>(this.reader.IsStartElement(localname, ns)); });
            this.skipFunc = new Func<Task>(() => { this.reader.Skip(); return Task.CompletedTask; });
            this.readFunc = new Func<Task<bool>>(() => { return Task.FromResult<bool>(this.reader.Read()); });
        }

        public override XmlNodeType NodeType
        {
            get
            {
                return this.reader.NodeType;
            }
        }

        public override string LocalName
        {
            get
            {
                return this.reader.LocalName;
            }
        }

        public override string NamespaceURI
        {
            get
            {
                return this.reader.NamespaceURI;
            }
        }

        public override string Prefix
        {
            get
            {
                return this.reader.Prefix;
            }
        }

        public override string Value
        {
            get
            {
                return this.reader.Value;
            }
        }

        public override int Depth
        {
            get
            {
                return this.reader.Depth;
            }
        }

        public override string BaseURI
        {
            get
            {
                return this.reader.BaseURI;
            }
        }

        public override bool IsEmptyElement
        {
            get
            {
                return this.reader.IsEmptyElement;
            }
        }

        public override int AttributeCount
        {
            get
            {
                return this.reader.AttributeCount;
            }
        }

        public override bool EOF
        {
            get
            {
                return this.reader.EOF;
            }
        }

        public override ReadState ReadState
        {
            get
            {
                return this.reader.ReadState;
            }
        }

        public override XmlNameTable NameTable
        {
            get
            {
                return this.reader.NameTable;
            }
        }

        public override string GetAttribute(string name)
        {
            return this.reader.GetAttribute(name);
        }

        public override string GetAttribute(string name, string namespaceURI)
        {
            return this.reader.GetAttribute(name, namespaceURI);
        }

        public override string GetAttribute(int i)
        {
            return this.reader.GetAttribute(i);
        }

        public override string LookupNamespace(string prefix)
        {
            return this.reader.LookupNamespace(prefix);
        }

        public override bool MoveToAttribute(string name)
        {
            return this.reader.MoveToAttribute(name);
        }

        public override bool MoveToAttribute(string name, string ns)
        {
            return this.reader.MoveToAttribute(name, ns);
        }

        public override bool MoveToElement()
        {
            return this.reader.MoveToElement();
        }

        public override bool MoveToFirstAttribute()
        {
            return this.reader.MoveToFirstAttribute();
        }

        public override bool MoveToNextAttribute()
        {
            return this.reader.MoveToNextAttribute();
        }

        public override bool Read()
        {
            return this.reader.Read();
        }

        public override bool ReadAttributeValue()
        {
            return this.reader.ReadAttributeValue();
        }

        public override void ResolveEntity()
        {
            this.reader.ResolveEntity();
        }

        public override Task<string> GetValueAsync()
        {
            return this.getValueFunc();
        }

        public Task<string> ReadElementStringAsync()
        {
            return this.readElementStringFunc();
        }

        public Task<string> ReadStringAsync()
        {
            return this.readStringFunc();
        }

        public Task ReadEndElementAsync()
        {
            return this.readEndElementFunc();
        }

        public override Task<XmlNodeType> MoveToContentAsync()
        {
            return this.moveToContentFunc();
        }

        public Task ReadStartElementAsync()
        {
            return this.readStartElementFunc();
        }

        public Task ReadStartElementAsync(string localname, string ns)
        {
            return this.readStartElementFunc2(localname, ns);
        }

        public Task<bool> IsStartElementAsync()
        {
            return this.isStartElementFunc();
        }

        public Task<bool> IsStartElementAsync(string localname, string ns)
        {
            return this.isStartElementFunc2(localname, ns);
        }

        public override Task SkipAsync()
        {
            return this.skipFunc();
        }

        public override Task<bool> ReadAsync()
        {
            return this.readFunc();
        }

        private static async Task ReadStartElementAsync(XmlReader reader)
        {
            if (await reader.MoveToContentAsync() != XmlNodeType.Element)
            {
                throw new InvalidOperationException(reader.NodeType.ToString() + " is an invalid XmlNodeType");
            }

            await reader.ReadAsync();
        }

        private static async Task ReadStartElementAsync(XmlReader reader, string name)
        {
            if (await reader.MoveToContentAsync() != XmlNodeType.Element)
            {
                throw new InvalidOperationException(reader.NodeType.ToString() + " is an invalid XmlNodeType");
            }
            if (reader.Name == name)
            {
                await reader.ReadAsync();
            }
            else
            {
                throw new InvalidOperationException("name doesn’t match");
            }
        }

        private static async Task ReadStartElementAsync(XmlReader reader, string localname, string ns)
        {
            if (await reader.MoveToContentAsync() != XmlNodeType.Element)
            {
                throw new InvalidOperationException(reader.NodeType.ToString() + " is an invalid XmlNodeType");
            }
            if ((reader.LocalName == localname) && (reader.NamespaceURI == ns))
            {
                await reader.ReadAsync();
            }
            else
            {
                throw new InvalidOperationException("localName or namespace doesn’t match");
            }
        }

        private static async Task ReadEndElementAsync(XmlReader reader)
        {
            if (await reader.MoveToContentAsync() != XmlNodeType.EndElement)
            {
                throw new InvalidOperationException();
            }
            await reader.ReadAsync();
        }

        private static async Task<bool> IsStartElementAsync(XmlReader reader)
        {
            return await reader.MoveToContentAsync() == XmlNodeType.Element;
        }

        private static async Task<bool> IsStartElementAsync(XmlReader reader, string name)
        {
            return await reader.MoveToContentAsync() == XmlNodeType.Element && reader.Name == name;
        }

        private static async Task<bool> IsStartElementAsync(XmlReader reader, string localname, string ns)
        {
            return await reader.MoveToContentAsync() == XmlNodeType.Element && reader.LocalName == localname && reader.NamespaceURI == ns;
        }

        private static async Task<string> ReadStringAsync(XmlReader reader)
        {
            if (reader.ReadState != ReadState.Interactive)
            {
                return string.Empty;
            }
            reader.MoveToElement();
            if (reader.NodeType == XmlNodeType.Element)
            {
                if (reader.IsEmptyElement)
                {
                    return string.Empty;
                }
                else if (!await reader.ReadAsync())
                {
                    throw new InvalidOperationException("Invalid operation");
                }
                if (reader.NodeType == XmlNodeType.EndElement)
                {
                    return string.Empty;
                }
            }
            string result = string.Empty;
            while (MyIsTextualNode(reader, reader.NodeType))
            {
                result += await reader.GetValueAsync();
                if (!await reader.ReadAsync())
                {
                    break;
                }
            }

            return result;
        }

        private static bool MyIsTextualNode(XmlReader reader, XmlNodeType nodeType)
        {
            const uint isTextualNodeBitmap = 0x6018;
            return 0 != (isTextualNodeBitmap & (1 << (int)nodeType));
        }

        private static async Task<string> ReadElementStringAsync(XmlReader reader)
        {
            string result = string.Empty;

            if (await reader.MoveToContentAsync() != XmlNodeType.Element)
            {
                //throw new XmlException(Res.Xml_InvalidNodeType, this.NodeType.ToString(), this as IXmlLineInfo);
                throw new XmlException();
            }

            if (!reader.IsEmptyElement)
            {
                await reader.ReadAsync();
                result = await ReadStringAsync(reader);
                if (reader.NodeType != XmlNodeType.EndElement)
                {
                    //throw new XmlException(Res.Xml_UnexpectedNodeInSimpleContent, new string[] { this.NodeType.ToString(), "ReadElementString" }, this as IXmlLineInfo);
                    throw new XmlException();
                }
                await reader.ReadAsync();
            }
            else
            {
                await reader.ReadAsync();
            }

            return result;
        }

        private static async Task<string> ReadElementStringAsync(XmlReader reader, string name)
        {
            string result = string.Empty;

            if (await reader.MoveToContentAsync() != XmlNodeType.Element)
            {
                throw new XmlException("InvalidNodeType");
            }
            if (reader.Name != name)
            {
                throw new XmlException("ElementNotFound");
            }

            if (!reader.IsEmptyElement)
            {
                result = await ReadStringAsync(reader);

                if (reader.NodeType != XmlNodeType.EndElement)
                {
                    throw new XmlException("InvalidNodeType");
                }
                await reader.ReadAsync();
            }
            else
            {
                await reader.ReadAsync();
            }
            return result;
        }

        private static async Task<string> ReadElementStringAsync(XmlReader reader, string localname, string ns)
        {
            string result = string.Empty;

            if (await reader.MoveToContentAsync() != XmlNodeType.Element)
            {
                throw new XmlException("InvalidNodeType");
            }

            if (reader.LocalName != localname || reader.NamespaceURI != ns)
            {
                throw new XmlException("ElementNotFound");
            }

            if (!reader.IsEmptyElement)
            {
                result = await ReadStringAsync(reader);

                if (reader.NodeType != XmlNodeType.EndElement)
                {
                    throw new XmlException("InvalidNodeType");
                }
                await reader.ReadAsync();
            }
            else
            {
                await reader.ReadAsync();
            }
            return result;
        }

        private static async Task DoWriteNodeAsync(XmlDictionaryWriter writer, XmlReader reader, bool defattr)
        {
            char[] writeNodeBuffer = null;
            const int WriteNodeBufferSize = 1024;

            if (null == reader)
            {
                throw new ArgumentNullException("reader");
            }

            bool canReadChunk = reader.CanReadValueChunk;
            int d = reader.NodeType == XmlNodeType.None ? -1 : reader.Depth;
            do
            {
                switch (reader.NodeType)
                {
                    case XmlNodeType.Element:
                        writer.WriteStartElement(reader.Prefix, reader.LocalName, reader.NamespaceURI);
                        writer.WriteAttributes(reader, defattr);
                        if (reader.IsEmptyElement)
                        {
                            writer.WriteEndElement();
                            break;
                        }
                        break;
                    case XmlNodeType.Text:
                        if (canReadChunk)
                        {
                            if (writeNodeBuffer == null)
                            {
                                writeNodeBuffer = new char[WriteNodeBufferSize];
                            }
                            int read;
                            while ((read = reader.ReadValueChunk(writeNodeBuffer, 0, WriteNodeBufferSize)) > 0)
                            {
                                writer.WriteChars(writeNodeBuffer, 0, read);
                            }
                        }
                        else
                        {
                            writer.WriteString(await reader.GetValueAsync());
                        }
                        break;

                    case XmlNodeType.Whitespace:
                    case XmlNodeType.SignificantWhitespace:
                        writer.WriteWhitespace(await reader.GetValueAsync());
                        break;

                    case XmlNodeType.CDATA:
                        writer.WriteCData(await reader.GetValueAsync());
                        break;

                    case XmlNodeType.EntityReference:
                        writer.WriteEntityRef(reader.Name);
                        break;

                    case XmlNodeType.XmlDeclaration:
                    case XmlNodeType.ProcessingInstruction:
                        writer.WriteProcessingInstruction(reader.Name, await reader.GetValueAsync());
                        break;

                    case XmlNodeType.DocumentType:
                        writer.WriteDocType(reader.Name, reader.GetAttribute("PUBLIC"), reader.GetAttribute("SYSTEM"), await reader.GetValueAsync());
                        break;

                    case XmlNodeType.Comment:
                        writer.WriteComment(await reader.GetValueAsync());
                        break;

                    case XmlNodeType.EndElement:
                        writer.WriteFullEndElement();
                        break;
                }
            } while (await reader.ReadAsync() && (d < reader.Depth || (d == reader.Depth && reader.NodeType == XmlNodeType.EndElement)));
        }
    }
}
