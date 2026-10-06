## Vì sao không nên chạy Docker trên Windows Server

- Đa số các image phổ biến đều là Linux, còn window rất ít
- Image Windows rất lớn (GB), cộng thêm server window tốn thêm giao diện
- Image Windows hay lỗi, linux ít hơn

## IIS - Internet Information Services

> Nhận request từ web (HTTP/HTTPS) và trả về nội dung trang web hay ứng dụng tương ứng.

### Cài IIS

1.  Chọn Manage → Add Roles and Features.
2.  Before You Begin: Nhấn Next.
3.  Installation type: Nhấn Next.
4.  Server Selection: Nhấn Next.
5.  Server Roles: Web Server (IIS).
6.  Features: Nhấn Next
7.  Web Server Role (IIS): Nhấn Next
    - Role Services: tick Application Development
        - .NET Extensibility 4.8: Add Features
        - ASP.NET 4.8: Add Features
        - ISAPI Extensions: tự có khi chọn ASP.NET 4.8
        - ISAPI Filters: tự có khi chọn ASP.NET 4.8

> nếu quên tick Application Development, thì đợi cài IIS xong chạy lệnh dưới

```bash
dism /online /enable-feature /featurename:IIS-NetFxExtensibility45 /all
dism /online /enable-feature /featurename:IIS-ASPNET45 /all
dism /online /enable-feature /featurename:IIS-ISAPIExtensions /all
dism /online /enable-feature /featurename:IIS-ISAPIFilter /all
iisreset
```

8. Confirmation: Install.
9. Đợi hoàn tất rồi nhấn Close.

### Kiểm tra IIS

Mở trình duyệt Edge trên Windows Server: http://localhost

Nếu xuất hiện trang IIS mặc định là cài thành công.

---

## SSL cloudfare

> video: https://www.youtube.com/watch?v=sVFnIUIaHjU

1. Tạo file CSR (private_key.csr) trên IIS
2. Đưa file CSR (private_key.csr) cho Cloudflare ký
3. Cloudflare trả certificate
4. IIS hoàn tất certificate
5. Gắn HTTPS vào website

### Bước 1: Tạo file CSR (private_key.csr) trên IIS

Trong IIS Manager:

1. Chọn **tên server** bên trái, không chọn website.
2. Mở **Server Certificates**.
3. Bên phải chọn **Create Certificate Request...**.

Điền:

- Common name: `vulebaolong.com`
- Organization: `vulebaolong.com`
- Organizational unit: `vlbl`
- City/locality: `Ho Chi Minh`
- State/province: `Ho Chi Minh`
- Country/region: `VN`

Nhấn **Next**, chọn:

- Cryptographic service provider:
    - Microsoft RSA SChannel Cryptographic Provider

- Bit length:
    - 2048

- Lưu file: `C:\ssl\vulebaolong-com.csr`

- Lúc này IIS đã tạo:
    - Private key: được giữ bí mật trong Windows.
    - CSR: file để gửi cho Cloudflare.

### Bước 2: Đưa file CSR (private_key.csr) cho Cloudflare ký

- Mở file CSR bằng Notepad. Nội dung có dạng:

```text
-----BEGIN CERTIFICATE REQUEST-----
...
-----END CERTIFICATE REQUEST-----
```

- Copy toàn bộ, bao gồm cả hai dòng `BEGIN` và `END`.

- Vào Cloudflare
    - SSL/TLS
    - Origin Server
    - Create Certificate
    - Ở màn hình đầu tiên, chọn: `Use my private key and CSR`
    - Sau đó dán CSR vừa copy vào.
    - Chọn hostname:

        ```text
        vulebaolong.com
        *.vulebaolong.com
        ```

    - Sau đó nhấn **Create**.

### Bước 3: Cloudflare trả certificate

- Chọn: DER
- Tải certificate và lưu và đổi đuổi `.der` thành `.cer`:
- `.cer`: chứa certificate/chuỗi certificate, không chứa private key.
- Private key đã nằm trong Windows từ lúc IIS tạo CSR.

### Bước 4: IIS hoàn tất certificate

IIS Manager

- tên server
- Server Certificates
- Complete Certificate Request...
    - Chọn file: `C:\ssl\vulebaolong-com.cer`
    - Friendly name: `Cloudflare Origin - vulebaolong.com`
    - Certificate store: `Personal`
    - Nhấn **OK**.

### Bước 5: Gắn HTTPS vào website

Chọn website vulebaolong.com

- Bindings...
- Add
- Điền:

    ```text
    Type: https
    IP address: All Unassigned
    Port: 443
    Host name: csharp-be-old.vulebaolong.com
    ```

- Bật: `Require Server Name Indication`
- SSL certificate: `Cloudflare Origin - vulebaolong.com`

Nhấn **OK**.

1. AWS Security Group mở port `443`.
2. DNS Cloudflare bật đám mây cam **Proxied**.
3. Cloudflare → SSL/TLS → Overview → chọn **Full (strict)**.

## URL Rewrite & Application Request Routing

> ARR yêu cầu URL Rewrite -> đúng thứ tự: URL Rewrite -> ARR

### Install

1. Microsoft IIS URL Rewrite 2.1

- https://www.iis.net/downloads/microsoft/url-rewrite -> English -> x64 installer

2. Microsoft Application Request Routing 3.0

- https://www.iis.net/downloads/microsoft/application-request-routing -> ARR 3.0 -> x64 installer

3. Mở lại / Reset IIS

```bash
 iisreset
```

4. Check

- Chọn tên server ở cột trái.
- Trong Features View cần thấy:
    - URL Rewrite
    - Application Request Routing Cache

### Thiết lập URL REWRITE

- chọn website (bên trái)
- chọn URL Rewrite
- chọn Add Rules (bên phải)
- Reverse Proxy
- OK
- Inbound Rules
    - localhost:3000 (url đang host của ứng dụng)
