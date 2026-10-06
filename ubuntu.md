## Connect

> Thông dụng nhất: Password (Mật khẩu) & SSH Key (Khóa bảo mật)

### In Web Browser

- EC2 Instance Connect (Dùng):
    - Kết nối trực tiếp bằng IP Public (cần mở cổng SSH/22).
    - Khi làm dự án cá nhân / Dev nhanh, server có IP Public.

- EC2 Instance Connect Endpoint:
    - Dùng mạng nội bộ (Private), không có IP Public người ngoài không vào được
    - AWS tạo ra Endpoint trung gian vào server

- SSM Session Manager:
    - Kết nối qua IAM, bảo mật nhất (không cần mở cổng SSH 22, không cần SSH Key).
    - Đóng 22, kết nối qua phần mềm SSM đã được cài trên server

- EC2 Serial Console:
    - Dùng để cứu hộ khẩn cấp khi server bị lỗi boot hoặc mất mạng hoàn toàn. Không thể vào bằng 3 cách trên.
    - EC2 -> EC2 Serial Console: Cần bật Allow

### In SSH Client

```bash
ssh -i "private_key.pem" ubuntu@IP_PUBLIC
```

- `SSH`: Secure (S) + Shell (SH)
- `-i`: (Identity file) đường dẫn tới file private key
- Password: Bỏ cờ -i, chỉ gõ tên user và IP: `ssh ubuntu@IP_PUBLIC`, Sau khi ấn Enter, màn hình sẽ hiện hỏi password

- `It is required that your private key files are NOT accessible by others`: Yêu cầu các tệp khóa riêng tư của bạn không được để người khác truy cập.
    - Group (g) và Other (o): Bắt buộc KHÔNG được có bất kỳ quyền nào
    - chmod 400: tối thiểu Owner - Read
    - cần chuyển quyền Owner(u): 4 (Read)

```bash
# tối thiểu Owner - Read
# Group (g) và Other (o): Bắt buộc KHÔNG được có bất kỳ quyền nào
chmod 400 "private_key.pem"
chmod u=r,g=,o= "private_key.pem"
```

```bash
vim -v
cat --version
nano --version
```

## SSH KEY trong ubuntu

> cả macOS và Windows (từ Windows 10 trở lên) đều được cài sẵn ssh-keygen

public key nằm ở file `.ssh/authorized_keys`

```bash
cd ~
cd .ssh
vim authorized_keys
```

### Tạo

```bash
# Tạo cặp khóa RSA
ssh-keygen -t rsa -b 4096 -C "devops_demo"

# Tạo cặp khóa Ed25519 ( hiện đại, ngắn hơn và bảo mật hơn RSA):
ssh-keygen -t ed25519 -C "devops_demo"
```

- `-t`: Type Loại thuật toán
- `-b`: Bits Độ dài khóa / Dung lượng, Chuẩn an toàn cho RSA hiện tại là 4096 bits
- `-C`: Comment (Ghi chú) thêm một đoạn văn bản ghi chú vào cuối file Public Key

- `Enter file in which to save the key (/Users/vulebaolong/.ssh/id_rsa):` duong_dan/ten_file, nếu không nhập sẽ lưu mặc định
- `Enter passphrase (empty for no passphrase): ` Passphrase như một "lớp khóa thứ hai". Nếu ai đó có Private Key họ vẫn không thể dùng nó để SSH vào server nếu không biết Passphrase này.
    - Nếu muốn tiện (Bỏ qua):
        - Bấm Enter (để trống).
        - SSH sẽ vào thẳng, không hỏi (passphrase) mật khẩu.
    - Nếu muốn bảo mật cao:
        - Gõ mật khẩu -> bấm Enter -> nhập lại lần 2 để xác nhận.
        - SSH sẽ yêu cầu nhập đúng (passphrase) mật khẩu.
- sẽ có 2 file:
    - 1 file private (không có đuôi)
    - 1 file public (.pub)

### Gắn

- copy nội dung public (.pub)
- paste vào ~/.ssh/authorized_keys
- dùng VIM: `vim ~/.ssh/authorized_keys`
  `o`: dòng mới + vào chế độ insert
  `G`: dòng cuối (sẽ đang ở dòng cuối vì chỉ có 1 pub)
  `$`: cuối dòng

## Docker

### Install

https://docs.docker.com/engine/install/ubuntu/

- chạy mọi lệnh trong doc

### Thêm user ubuntu vào docker

permission denied while trying to connect to the docker API at unix:///var/run/docker.sock

> Mặc định khi mới cài đặt, Docker daemon chạy dưới quyền root
> Docker chỉ cho user root và các user thuộc nhóm docker truy cập
> Nên User ubuntu chưa có quyền đọc/ghi file socket của Docker (/var/run/docker.sock).

```bash
# lệnh chuẩn hệ thống
sudo usermod -aG docker ubuntu
# -a: append
# -G: group

# lệnh cấp cao
sudo adduser ubuntu docker

# Cập nhật lại quyền cho phiên làm việc hiện tại
newgrp docker
# HOẶC đăng xuất rồi đăng nhập lại
```

## Đưa source/image lên server

## source
- git
- cp
- FileZilla
- Terminus
## image (thường dùng với CI/CD)
- docker hub
- docker save, docker load `lưu image ra file tar`
