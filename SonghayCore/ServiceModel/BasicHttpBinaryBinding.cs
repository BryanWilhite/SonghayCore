#if NET452 || NET462

using System;
using System.Globalization;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace Songhay.ServiceModel
{
    /// <summary>
    /// Represents a basic http binding with binary encoding.
    /// </summary>
    /// <remarks>
    /// For more, see “BasicHttpBinaryBinding for Silverlight”
    /// by Beat Kiener
    /// [http://blog.thekieners.com/2009/05/22/basichttpbinarybinding-for-silverlight/]
    /// </remarks>
    public class BasicHttpBinaryBinding : BasicHttpBinding
    {
        /// <summary>
        ///  Initializes a new instance of the BasicHttpBinaryBinding class.
        /// </summary>
        public BasicHttpBinaryBinding()
            : this(BasicHttpSecurityMode.None)
        {
        }

        /// <summary>
        /// Initializes a new instance of the BasicHttpBinaryBinding class.
        /// </summary>
        /// <param name="securityMode">
        /// The value of System.ServiceModel.BasicHttpSecurityMode that specifies
        /// the type of security that is used with the SOAP message and for the client.
        /// </param>
        public BasicHttpBinaryBinding(BasicHttpSecurityMode securityMode)
            : this(securityMode, true)
        {
        }

        /// <summary>
        /// Initializes a new instance of the BasicHttpBinaryBinding class.
        /// </summary>
        /// <param name="securityMode">
        /// The value of System.ServiceModel.BasicHttpSecurityMode that specifies
        /// the type of security that is used with the SOAP message and for the client.
        /// </param>
        /// <param name="binaryEncoding">
        /// Indicates whether the binary encoding is enabled or not
        /// </param>
        public BasicHttpBinaryBinding(BasicHttpSecurityMode securityMode, bool binaryEncoding)
            : base(securityMode)
        {
            this.BinaryEncoding = true;
            this.BinaryEncoding = binaryEncoding;
        }

        /// <summary>
        /// Gets or sets a value that indicates whether the binary encoding is enabled or not. Default is true.
        /// </summary>
        public bool BinaryEncoding { get; set; }

        /// <summary>
        /// Returns an ordered collection of binding elements contained in the current binding.
        /// </summary>
        public override BindingElementCollection CreateBindingElements()
        {
            BindingElementCollection elements = base.CreateBindingElements();

            if(this.BinaryEncoding)
            {
                // search the existing message encoding element (Text or MTOM) and replace it
                // note: the search must be done with the base type of text and mtom binding element,
                // because this code is compiled against silverlight also
                // and there is no mtom encoding available
                for(int i = elements.Count - 1; i >= 0; i--)
                {
                    BindingElement element = elements[i];
                    if(element.GetType().IsSubclassOf(typeof(MessageEncodingBindingElement)))
                    {
                        BinaryMessageEncodingBindingElement binaryElement = null;

                        var textEncoding = element as TextMessageEncodingBindingElement;
                        var mtomEncoding = element as MtomMessageEncodingBindingElement;

                        if(textEncoding != null)
                        {
                            // copy settings to binary element
                            binaryElement = new BinaryMessageEncodingBindingElement();

                            // copy settings
                            binaryElement.ReaderQuotas.MaxArrayLength = textEncoding.ReaderQuotas.MaxArrayLength;
                            binaryElement.ReaderQuotas.MaxBytesPerRead = textEncoding.ReaderQuotas.MaxBytesPerRead;
                            binaryElement.ReaderQuotas.MaxDepth = textEncoding.ReaderQuotas.MaxDepth;
                            binaryElement.ReaderQuotas.MaxNameTableCharCount = textEncoding.ReaderQuotas.MaxNameTableCharCount;
                            binaryElement.ReaderQuotas.MaxStringContentLength = textEncoding.ReaderQuotas.MaxStringContentLength;
                            binaryElement.MaxReadPoolSize = textEncoding.MaxReadPoolSize;
                            binaryElement.MaxWritePoolSize = textEncoding.MaxWritePoolSize;

                            // binary uses always soap-1.2
                            //binaryElement.MessageVersion = textEncoding.MessageVersion;
                        }

                        else if(mtomEncoding != null)
                        {
                            // copy settings to binary element
                            binaryElement = new BinaryMessageEncodingBindingElement();

                            // copy settings
                            binaryElement.ReaderQuotas.MaxArrayLength = mtomEncoding.ReaderQuotas.MaxArrayLength;
                            binaryElement.ReaderQuotas.MaxBytesPerRead = mtomEncoding.ReaderQuotas.MaxBytesPerRead;
                            binaryElement.ReaderQuotas.MaxDepth = mtomEncoding.ReaderQuotas.MaxDepth;
                            binaryElement.ReaderQuotas.MaxNameTableCharCount = mtomEncoding.ReaderQuotas.MaxNameTableCharCount;
                            binaryElement.ReaderQuotas.MaxStringContentLength = mtomEncoding.ReaderQuotas.MaxStringContentLength;
                            binaryElement.MaxReadPoolSize = mtomEncoding.MaxReadPoolSize;
                            binaryElement.MaxWritePoolSize = mtomEncoding.MaxWritePoolSize;

                            // binary uses always soap-1.2
                            //binaryElement.MessageVersion = mtomEncoding.MessageVersion;
                        }

                        else if(element is BinaryMessageEncodingBindingElement)
                        {
                            // it's already binary
                        }
                        else
                        {
                            string exStr = string.Format(CultureInfo.CurrentCulture, "Message encoding type {0} is not implemented.", element.GetType().Name);
                            throw new NotImplementedException(exStr);
                        }

                        if(binaryElement != null)
                        {
                            elements.RemoveAt(i);
                            elements.Insert(i, binaryElement);
                            break;
                        }
                    }
                }
            }

            return elements;
        }
    }
}

#endif
