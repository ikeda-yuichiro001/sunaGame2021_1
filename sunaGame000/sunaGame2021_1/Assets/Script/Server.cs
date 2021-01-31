using System;
using System.Text;
using System.Net;
using System.IO;
using System.Net.NetworkInformation;
using System.Net.Http;

public class Server
{
    //デリゲートの型宣言-----------------------------------
    public delegate void TaskEvent();
    public delegate void ErrorEvent(string text);


    //パラメータ関連 --------------------------------------
    public string useraddress;
    public string password;
    public string serveraddress;

    private string accessUrlBase = "";
    private WebClient webClient;
    private ICredentials icr; 

    //コンストラクタ -------------------------------------
    public Server(string useraddress, string password, string serveraddress)
    {
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;
        this.useraddress = useraddress;
        this.password = password;
        this.serveraddress = serveraddress;
        this.accessUrlBase = "ftp://" + useraddress + "/%2f";
        icr = new NetworkCredential(useraddress, password);
        webClient = new WebClient() { Credentials = new NetworkCredential(useraddress, password) };
    }
   
    //デストラクタ関連  ----------------------------------
    ~Server()
    {
         Dispose();
    }

    public void Dispose()
    {
        webClient?.Dispose();
        icr = null;
        useraddress = null;
        password = null;
        serveraddress = null;
        accessUrlBase = null;
    }


    //愉快な関数たち -------------------------------------------------------------------

    public bool CanConectNetWork()
    {
        return NetworkInterface.GetIsNetworkAvailable();
    }

    public string GetText(string url, string MissingReturn = "N/A")
    {
        if (CanConectNetWork()) //インターネット接続できるなら...
            try
            {
                string t = webClient.DownloadString(accessUrlBase + url);
                return t; //ネット上の現在のバージョンを取得
            }
            catch (WebException)
            {
            }

        return MissingReturn;
    }



    public void FileDownload(string local, string url, TaskEvent finished = null, ErrorEvent error = null)
    {
        try
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;
            webClient.DownloadFile(accessUrlBase + url, local);
            finished?.Invoke();
        }
        catch (WebException e)
        {
            error?.Invoke(((FtpWebResponse)e.Response).StatusDescription);
        }
        catch (Exception e)
        {
            error?.Invoke(e.Message);
        }
    }



    public void FileUpload(string local, string url, TaskEvent finished = null, ErrorEvent error = null)
    {
        try
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;
            webClient.UploadFile(accessUrlBase + url, local);
            finished?.Invoke();
        }
        catch (WebException e)
        {
            error?.Invoke(((FtpWebResponse)e.Response).StatusDescription);
        }
        catch (Exception e)
        {
            error?.Invoke(e.Message);
        }
    }


    public void FileUploadStr(string url, string data, TaskEvent finished = null, ErrorEvent error = null)
    {
        try
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;
            webClient.UploadString(accessUrlBase + url, data);
            finished?.Invoke();
        }
        catch (WebException e)
        {
            error?.Invoke(((FtpWebResponse)e.Response).StatusDescription);
        }
        catch (Exception e)
        {
            error?.Invoke(e.Message);
        }
    }


    public void FileAddWrite(string text, string url, TaskEvent finished = null, ErrorEvent error = null)
    {
        try
        {
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(new Uri(accessUrlBase + url));
            request.Method = WebRequestMethods.Ftp.AppendFile;

            byte[] fileContents = Encoding.UTF8.GetBytes(text);

            request.ContentLength = fileContents.Length;
            request.Credentials = icr;

            Stream requestStream = request.GetRequestStream();
            requestStream.Write(fileContents, 0, fileContents.Length);
            requestStream.Close();
            FtpWebResponse response = (FtpWebResponse)request.GetResponse();
            response.Close();
            requestStream.Dispose();
            response.Dispose();
            finished?.Invoke();
        }
        catch (WebException e)
        {
            error?.Invoke(((FtpWebResponse)e.Response).StatusDescription);
        }
    }


    public void MakeDirectory(string url, TaskEvent finished = null, ErrorEvent error = null)
    {
        try
        {
            FtpWebRequest ftpReq = (FtpWebRequest)WebRequest.Create(accessUrlBase + url);
            ftpReq.Credentials = icr; 
            ftpReq.Method = WebRequestMethods.Ftp.MakeDirectory;
            FtpWebResponse ftpRes = (FtpWebResponse)ftpReq.GetResponse();
            ftpRes.Close();
            finished?.Invoke();
        }
        catch (WebException e)
        {
            error?.Invoke(((FtpWebResponse)e.Response).StatusDescription);
        }
        catch (Exception e)
        {
            error?.Invoke(e.Message);
        }
    }



    public void Delete(string url, TaskEvent finished = null, ErrorEvent error = null)
    {
        try
        {
            FtpWebRequest ftpReq = (FtpWebRequest)WebRequest.Create(accessUrlBase + url);
            ftpReq.Credentials = icr;
            ftpReq.Method = WebRequestMethods.Ftp.DeleteFile;
            FtpWebResponse ftpRes = (FtpWebResponse)ftpReq.GetResponse();
            ftpRes.Close();
        }
        catch (WebException e)
        {
            error?.Invoke(((FtpWebResponse)e.Response).StatusDescription);
        }
        catch (Exception e)
        {
            error?.Invoke(e.Message);
        }
    }


    //未確定
    public bool HasURL(string url)
    {
        string c = Guid.NewGuid().ToString();
        return GetText(url, c) != c;// HasURL_Raw("http" + accessUrlBase.Remove(0,3) + url);
    }

    public bool HasURL_Raw(string url)
    {
        if (CanConectNetWork()) //インターネット接続できるなら...
            try
            {
                webClient.DownloadString(url);
                return true; //ネット上の現在のバージョンを取得
            }
            catch (WebException)
            {
            }
            catch (Exception)
            {
            }

        return false;
    }

    public HttpStatusCode GetStatusCode(string url)
    {
        using (var client = new HttpClient())
            return client.GetAsync(url).Result.StatusCode;
    }

    public void MkFile(string url, TaskEvent finished = null, ErrorEvent error = null)
    {
        try
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;
            webClient.UploadString(accessUrlBase + url, "");
            finished?.Invoke();
        }
        catch (WebException e)
        {
            error?.Invoke(((FtpWebResponse)e.Response).StatusDescription);
        }
        catch (Exception e)
        {
            error?.Invoke(e.Message);
        }
    }


}


/* 仕様
 コンストラクタ------------------------------------------------------------
　AccesserFTP(string useraddress, string password, string serveraddress)
    useraddress = FTC通信を行う際のアカウント
    password    = FTC通信を行う際のアカウントのパスワード
    serveraddress = Serverのドメイン
 ---------------------------------------------------------------------------

※以後urlという引数名は　public_html/??? の???の部分からとなる。
※以後localという引数名は　ローカルの絶対パスを示す。

TaskEvent型 ・・・　引数なしのvoid型関数を引数として扱える
ErrorEvent型 ・・・　string型の引数を１つだけもつvoid型関数を引数として扱える


関数------------------------------------------------------------------------------------------
Dispose() ・・・説明不要!!

CanConectNetWork() ・・・ネットに接続できるかbool型で返す。


GetText(string url, [string MissingReturn = "N/A"])
    MissingReturn = 何らかの理由により取得できない場合に返す値


FileDownload(string local, string url, [TaskEvent finished = null], [ErrorEvent error = null])
    ファイルをlocalにダウンロードする。


FileUpload(string local, string url, TaskEvent finished = null, ErrorEvent error = null)
    urlのファイルをlocalのファイルで置き換える。(Urlのファイルがない場合新規作成される。
    ある場合上書き)

FileAddWrite(string text, string url, TaskEvent finished = null, ErrorEvent error = null)
    urlのファイルにtextを末尾に追加します。


MakeDirectory(string url, TaskEvent finished = null, ErrorEvent error = null)
    urlのディレクトリを作成します。(urlの引数の末尾に/はいりません。てかいれんな)

Delete(string url,TaskEvent finished = null, ErrorEvent error = null)
    ファイルを削除します。
    <注意>アプリケーションがデコンパイルされIDやパスワードが漏洩するとこのメソッドでサーバーごと削除される危険があります。
    そのため使用しない場合このメソッドを削除してください。(まぁ、漏洩した時点で詰みではありますが...)

     
 HasURL(string url) 
   urlが存在するかbool型で返します。

 HasURL_Raw(string url)
   HasURLとの違いはurlの引数が絶対パス? であることです。
   自分のサーバー以外にもアクセスし存在するか返します。

 GetStatusCode(string url)
  　urlのパスにアクセスしそのリターンをHttpStatusCode型で返します。  
   ------------------------------------------------------------------------------------------
*/
