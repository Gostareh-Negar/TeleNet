using System;
using System.Collections.Generic;
using System.Text;

namespace MTProto.NET.Server.Infrastructure.Encryption
{
	/// <summary>
	/// https://stackoverflow.com/questions/9607295/calculate-rsa-key-fingerprint
	///  ssh-keygen -f tdlib.server.local
	///  ssh-keygen -f tdlib.server.local -e -m pem >tdlib.server.local.pem
	///  ssh-keygen -lf tdlib.server.local.pub
	///  ssh-keygen -E md5 -lf tdlib.server.local.pub >tdlib.server.local.pub.fingerprint.md5
	/// </summary>
	class DefaultKey 
	{
		#region HardCoded
		public static string FingerPrint = @"
2048 SHA256:5XT44RxLdr7iM2stTErK09oOwu32uPxsOt7NQiIglRM gnco\babak@BabakHP (RSA)";
		public static string FingerPrint_MD5 = @"
2048 MD5:4b:ea:6d:96:40:5c:6d:d8:b7:d0:aa:19:9d:b0:8f:fd gnco\babak@BabakHP (RSA)";

		//-----BEGIN RSA PRIVATE KEY-----
		public static string PRIVATE_PEM = @"
MIIEowIBAAKCAQEA06j7GbSjSB7mj4az+GrkbNgP19zwy63mL+73kuspoNRFdbhd
R4kcLyBdIy8WhSqK0l0bSglxzhqd8nNNu1lPZbgTBwUZMb/JyFvuKTiJ+QjSr9d9
tc0e9vZ+kjpCZxUtFH/wxQ/f3JU5IYVYmbp9xAniVNZO90bI3nugEvDVAEQ1bsiO
5JVWTLUFk0YkwAbvIE45elI1JlIhfq3x4muJrDxjZV1DASxs8gxva8gYMaL04rfM
XbTavXuxEjOUk7a6qiN3jyzRj2T7Zcc4/iFd3Vne6bPjKcHqyxbl8MNmIssvS65G
c0PqSOhzrX5RmgMSmw/rn4QyV664hhGibhbQ+QIDAQABAoIBAQCHA7qxKg4h+jwe
f9GbfCwb1jM9Al3DvzkfiHL6j/Gs+tsX/bPa5vZIhX+D6wyVg46sI+I9dwrWaxon
xy9le9Hu092nU7Q/jNSdby7bsooohl1G99Hjv2WqKZCRnIIxGUv5UKd4POkS30lj
PZlH7rM88wUiEtaqW8fUS83PZzqoALE5ynrGrc0YT1VzDFXHjIDY2V5JuZ8NPyYs
WTHz3JAysDgY9X/V3lxmXKE4FnhUByd636gI16N29CP1V25sQfVu+HOqYAfUDgQ/
d32tPcx3VoBVSVwRLZZzL5+U6NLVKxyI9Fi2QXleehNKgd4M7PNV3p+YzVdAAh5s
JDgh7wxlAoGBAPQbvPwF1VD4xgF1yTbYhmCHjkXIJfIhrDobHYHGAVSycdx529LI
oKkmADKmFJkWqRuQgZfeiTd/lt0uzjfdkgGeYdUG7CeEA1pd63jaApVCNi0bmPqy
dRwoeuq3bckTkXEvtC7RfKw+XQTow2RvJGsv07vb1cD4MLYbyNP2Ve2/AoGBAN34
lQD14bulrpi3R3bww/t5LlSKjzctAHbYVx/qQkSboYrpVx5zA3f7Sq7aEgTX4D+1
bsatjZos1GFLB9GEKnTb+u+xCC80ACgfO+pwXPcT5GuyTjdUV5S7LVDZFIIb+CEa
p6nkYNBkHOO+0FGx4/lOQhZBtom8qV5WAtkY+F9HAoGAH/GBSXl58J6dSpOfQQ4U
h401kOwgCw9c+j2SHulKQ0sWm0NXAL5AR+IxJG+XQnE0r+a5DqaQTjLkCw2U7rqP
4KZZJwdA3+rmhWzE44ujyuRfMofp/ORdbtHdQ9m9BBMLdURIz9eZ+PAwO/Q5nWrt
2RjpHCwoTJgtx6bbIOGbXQECgYBHjKpTDe7+cpCOD34Mu90ggVZK0AMMEQh2RpQG
6JcF0HjI8yAu43n5wdm39Pkb8I4LWytjBWyAhvTKi8nwYj7hPktr2c++j2+Bf1fr
4N5GknXttSL7OMemXJNl3SN8MRHRcesbm08NvUeGreouez32DaDF1dbGHTcm/mia
nC7rXwKBgDsqFvDlWJ1Q2Wa7ZfezyKdf9aSOoWxFGXTcaIVACApVDpljIoWtXbyb
TUVoYS+Ovl5LgDHSfcqLpnzFCi5todA3OEXyTG2jhPcuUn5tPZuL22K096cTDF98
QpsRjfSuRSuVwSGny1DN4a0RfB08nymdCzVcz2sFaPhuZmHiQEVa


";
		//-----END RSA PRIVATE KEY-----
		//-----BEGIN RSA PUBLIC KEY-----
		public static string PUBLIC_PEM = @"
MIIBCgKCAQEA06j7GbSjSB7mj4az+GrkbNgP19zwy63mL+73kuspoNRFdbhdR4kc
LyBdIy8WhSqK0l0bSglxzhqd8nNNu1lPZbgTBwUZMb/JyFvuKTiJ+QjSr9d9tc0e
9vZ+kjpCZxUtFH/wxQ/f3JU5IYVYmbp9xAniVNZO90bI3nugEvDVAEQ1bsiO5JVW
TLUFk0YkwAbvIE45elI1JlIhfq3x4muJrDxjZV1DASxs8gxva8gYMaL04rfMXbTa
vXuxEjOUk7a6qiN3jyzRj2T7Zcc4/iFd3Vne6bPjKcHqyxbl8MNmIssvS65Gc0Pq
SOhzrX5RmgMSmw/rn4QyV664hhGibhbQ+QIDAQAB";
		//		-----END RSA PUBLIC KEY-----
		public static string c_hardcodedDhPrime =
								"c71caeb9c6b1c9048e6c522f70f13f73980d40238e3e21c14934d037563d930f" +
								"48198a0aa7c14058229493d22530f4dbfa336f6e0ac925139543aed44cce7c37" +
								"20fd51f69458705ac68cd4fe6b6b13abdc9746512969328454f18faf8c595f64" +
								"2477fe96bb2a941d5bcd1d4ac8cc49880708fa9b378e3c4f3a9060bee67cf9a4" +
								"a4a695811051907e162753b56b0f6b410dba74d8a84b2a14b3144e0ef1284754" +
								"fd17ed950d5965b4b9dd46582db1178d169c6bc465b0d6ff9ca3928fef5b9ae4" +
								"e418fc15e83ebea0f87fa9ff5eed70050ded2849f47bf959d956850ce929851f" +
								"0d8115f635b105ee2e4e15d04b2454bf6f4fadf034b10403119cd8e3b92fcc5b";
		#endregion
		public string PrivatePem => PRIVATE_PEM;

		public string DhPrime => c_hardcodedDhPrime;
	}
}
