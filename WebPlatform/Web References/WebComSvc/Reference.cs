﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.42000
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

// 
// 此源代码是由 Microsoft.VSDesigner 4.0.30319.42000 版自动生成。
// 
#pragma warning disable 1591

namespace WebPlatform.WebComSvc {
    using System;
    using System.Web.Services;
    using System.Diagnostics;
    using System.Web.Services.Protocols;
    using System.Xml.Serialization;
    using System.ComponentModel;
    
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1055.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Web.Services.WebServiceBindingAttribute(Name="SerialPortDataSoap", Namespace="http://tempuri.org/")]
    public partial class SerialPortData : System.Web.Services.Protocols.SoapHttpClientProtocol {
        
        private System.Threading.SendOrPostCallback PortStatusOperationCompleted;
        
        private System.Threading.SendOrPostCallback OpenPortOperationCompleted;
        
        private System.Threading.SendOrPostCallback ClosePortOperationCompleted;
        
        private System.Threading.SendOrPostCallback SendDataOperationCompleted;
        
        private System.Threading.SendOrPostCallback GetReciveDataOperationCompleted;
        
        private bool useDefaultCredentialsSetExplicitly;
        
        /// <remarks/>
        public SerialPortData() {
            this.Url = global::WebPlatform.Properties.Settings.Default.WebPlatform_WebComSvc_SerialPortData;
            if ((this.IsLocalFileSystemWebService(this.Url) == true)) {
                this.UseDefaultCredentials = true;
                this.useDefaultCredentialsSetExplicitly = false;
            }
            else {
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }
        
        public new string Url {
            get {
                return base.Url;
            }
            set {
                if ((((this.IsLocalFileSystemWebService(base.Url) == true) 
                            && (this.useDefaultCredentialsSetExplicitly == false)) 
                            && (this.IsLocalFileSystemWebService(value) == false))) {
                    base.UseDefaultCredentials = false;
                }
                base.Url = value;
            }
        }
        
        public new bool UseDefaultCredentials {
            get {
                return base.UseDefaultCredentials;
            }
            set {
                base.UseDefaultCredentials = value;
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }
        
        /// <remarks/>
        public event PortStatusCompletedEventHandler PortStatusCompleted;
        
        /// <remarks/>
        public event OpenPortCompletedEventHandler OpenPortCompleted;
        
        /// <remarks/>
        public event ClosePortCompletedEventHandler ClosePortCompleted;
        
        /// <remarks/>
        public event SendDataCompletedEventHandler SendDataCompleted;
        
        /// <remarks/>
        public event GetReciveDataCompletedEventHandler GetReciveDataCompleted;
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/PortStatus", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public bool PortStatus() {
            object[] results = this.Invoke("PortStatus", new object[0]);
            return ((bool)(results[0]));
        }
        
        /// <remarks/>
        public void PortStatusAsync() {
            this.PortStatusAsync(null);
        }
        
        /// <remarks/>
        public void PortStatusAsync(object userState) {
            if ((this.PortStatusOperationCompleted == null)) {
                this.PortStatusOperationCompleted = new System.Threading.SendOrPostCallback(this.OnPortStatusOperationCompleted);
            }
            this.InvokeAsync("PortStatus", new object[0], this.PortStatusOperationCompleted, userState);
        }
        
        private void OnPortStatusOperationCompleted(object arg) {
            if ((this.PortStatusCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.PortStatusCompleted(this, new PortStatusCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/OpenPort", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public bool OpenPort() {
            object[] results = this.Invoke("OpenPort", new object[0]);
            return ((bool)(results[0]));
        }
        
        /// <remarks/>
        public void OpenPortAsync() {
            this.OpenPortAsync(null);
        }
        
        /// <remarks/>
        public void OpenPortAsync(object userState) {
            if ((this.OpenPortOperationCompleted == null)) {
                this.OpenPortOperationCompleted = new System.Threading.SendOrPostCallback(this.OnOpenPortOperationCompleted);
            }
            this.InvokeAsync("OpenPort", new object[0], this.OpenPortOperationCompleted, userState);
        }
        
        private void OnOpenPortOperationCompleted(object arg) {
            if ((this.OpenPortCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.OpenPortCompleted(this, new OpenPortCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/ClosePort", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public bool ClosePort() {
            object[] results = this.Invoke("ClosePort", new object[0]);
            return ((bool)(results[0]));
        }
        
        /// <remarks/>
        public void ClosePortAsync() {
            this.ClosePortAsync(null);
        }
        
        /// <remarks/>
        public void ClosePortAsync(object userState) {
            if ((this.ClosePortOperationCompleted == null)) {
                this.ClosePortOperationCompleted = new System.Threading.SendOrPostCallback(this.OnClosePortOperationCompleted);
            }
            this.InvokeAsync("ClosePort", new object[0], this.ClosePortOperationCompleted, userState);
        }
        
        private void OnClosePortOperationCompleted(object arg) {
            if ((this.ClosePortCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.ClosePortCompleted(this, new ClosePortCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/SendData", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public bool SendData(string data, string deviceID) {
            object[] results = this.Invoke("SendData", new object[] {
                        data,
                        deviceID});
            return ((bool)(results[0]));
        }
        
        /// <remarks/>
        public void SendDataAsync(string data, string deviceID) {
            this.SendDataAsync(data, deviceID, null);
        }
        
        /// <remarks/>
        public void SendDataAsync(string data, string deviceID, object userState) {
            if ((this.SendDataOperationCompleted == null)) {
                this.SendDataOperationCompleted = new System.Threading.SendOrPostCallback(this.OnSendDataOperationCompleted);
            }
            this.InvokeAsync("SendData", new object[] {
                        data,
                        deviceID}, this.SendDataOperationCompleted, userState);
        }
        
        private void OnSendDataOperationCompleted(object arg) {
            if ((this.SendDataCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.SendDataCompleted(this, new SendDataCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/GetReciveData", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string GetReciveData() {
            object[] results = this.Invoke("GetReciveData", new object[0]);
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void GetReciveDataAsync() {
            this.GetReciveDataAsync(null);
        }
        
        /// <remarks/>
        public void GetReciveDataAsync(object userState) {
            if ((this.GetReciveDataOperationCompleted == null)) {
                this.GetReciveDataOperationCompleted = new System.Threading.SendOrPostCallback(this.OnGetReciveDataOperationCompleted);
            }
            this.InvokeAsync("GetReciveData", new object[0], this.GetReciveDataOperationCompleted, userState);
        }
        
        private void OnGetReciveDataOperationCompleted(object arg) {
            if ((this.GetReciveDataCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.GetReciveDataCompleted(this, new GetReciveDataCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        public new void CancelAsync(object userState) {
            base.CancelAsync(userState);
        }
        
        private bool IsLocalFileSystemWebService(string url) {
            if (((url == null) 
                        || (url == string.Empty))) {
                return false;
            }
            System.Uri wsUri = new System.Uri(url);
            if (((wsUri.Port >= 1024) 
                        && (string.Compare(wsUri.Host, "localHost", System.StringComparison.OrdinalIgnoreCase) == 0))) {
                return true;
            }
            return false;
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1055.0")]
    public delegate void PortStatusCompletedEventHandler(object sender, PortStatusCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1055.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class PortStatusCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal PortStatusCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public bool Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((bool)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1055.0")]
    public delegate void OpenPortCompletedEventHandler(object sender, OpenPortCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1055.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class OpenPortCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal OpenPortCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public bool Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((bool)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1055.0")]
    public delegate void ClosePortCompletedEventHandler(object sender, ClosePortCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1055.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class ClosePortCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal ClosePortCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public bool Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((bool)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1055.0")]
    public delegate void SendDataCompletedEventHandler(object sender, SendDataCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1055.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class SendDataCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal SendDataCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public bool Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((bool)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1055.0")]
    public delegate void GetReciveDataCompletedEventHandler(object sender, GetReciveDataCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1055.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class GetReciveDataCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal GetReciveDataCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public string Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }
}

#pragma warning restore 1591