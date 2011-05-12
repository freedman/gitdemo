using System;
using System.Configuration;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text;

using Amazon;
using Amazon.EC2;
using Amazon.EC2.Model;
using Amazon.SimpleDB;
using Amazon.SimpleDB.Model;
using Amazon.S3;
using Amazon.S3.Model;

namespace AWS_Console_App1
{
    class Program
    {
        public static void Main(string[] args)
        {
            Console.Write(GetServiceOutput());
            Console.Read();
        }

        public static string GetServiceOutput()
        {
            var sb = new StringBuilder(1024);
            using (var sr = new StringWriter(sb))
            {
                var appConfig = ConfigurationManager.AppSettings;

                sr.WriteLine("===========================================");
                sr.WriteLine("Welcome to the AWS .NET SDK!");
                sr.WriteLine("===========================================");

                // Print the number of Amazon EC2 instances.
                var ec2 = AWSClientFactory.CreateAmazonEC2Client(
                    appConfig["AWSAccessKey"],
                    appConfig["AWSSecretKey"]//,
                    , new AmazonEC2Config().WithServiceURL("https://eu-west-1.ec2.amazonaws.com")
                    );

                var ec2Request = new RunInstancesRequest
                                     {
                                         KeyName = "FIQA",
                                         ImageId = ";" ,
                                         InstanceType = "m1.xlarge",
                                         MinCount = 1,
                                         MaxCount = 1,
                                         Placement = new Placement
                                             {
                                                 AvailabilityZone = "eu-west-1c"
                                             }
                                     };
                ec2Request.SecurityGroup.Add("FIQA");
                ec2Request.BlockDeviceMapping.Add(
                    new BlockDeviceMapping
                        {
                            DeviceName = "/dev/sda1",
                            Ebs = new EbsBlockDevice
                                      {
                                          VolumeSize = 200
                                      }
                        });
                try
                {
                    var ec2Response = ec2.RunInstances(ec2Request);
                    Console.WriteLine(ec2Response.ToXML());
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    //throw;
                }


                //var ec2Request = new DescribeInstancesRequest();

                //try
                //{
                //    var ec2Response = ec2.DescribeInstances(ec2Request);
                //    int numInstances = 0;
                //    numInstances = ec2Response.DescribeInstancesResult.Reservation.Count;
                //    sr.WriteLine("You have " + numInstances + " Amazon EC2 instance(s) running in the US-East (Northern Virginia) region.");

                //}
                //catch (AmazonEC2Exception ex)
                //{
                //    if (ex.ErrorCode != null && ex.ErrorCode.Equals("AuthFailure"))
                //    {
                //        sr.WriteLine("The account you are using is not signed up for Amazon EC2.");
                //        sr.WriteLine("You can sign up for Amazon EC2 at http://aws.amazon.com/ec2");
                //    }
                //    else
                //    {
                //        sr.WriteLine("Caught Exception: " + ex.Message);
                //        sr.WriteLine("Response Status Code: " + ex.StatusCode);
                //        sr.WriteLine("Error Code: " + ex.ErrorCode);
                //        sr.WriteLine("Error Type: " + ex.ErrorType);
                //        sr.WriteLine("Request ID: " + ex.RequestId);
                //        sr.WriteLine("XML: " + ex.XML);
                //    }
                //}
                //sr.WriteLine();

                //    // Print the number of Amazon SimpleDB domains.
                //    var sdb = AWSClientFactory.CreateAmazonSimpleDBClient(
                //        appConfig["AWSAccessKey"],
                //        appConfig["AWSSecretKey"]
                //        );
                //    var sdbRequest = new ListDomainsRequest();

                //    try
                //    {
                //        var sdbResponse = sdb.ListDomains(sdbRequest);

                //        if (sdbResponse.IsSetListDomainsResult())
                //        {
                //            int numDomains = 0;
                //            numDomains = sdbResponse.ListDomainsResult.DomainName.Count;
                //            sr.WriteLine("You have " + numDomains + " Amazon SimpleDB domain(s) in the US-East (Northern Virginia) region.");
                //        }
                //    }
                //    catch (AmazonSimpleDBException ex)
                //    {
                //        if (ex.ErrorCode != null && ex.ErrorCode.Equals("AuthFailure"))
                //        {
                //            sr.WriteLine("The account you are using is not signed up for Amazon SimpleDB.");
                //            sr.WriteLine("You can sign up for Amazon SimpleDB at http://aws.amazon.com/simpledb");
                //        }
                //        else
                //        {
                //            sr.WriteLine("Caught Exception: " + ex.Message);
                //            sr.WriteLine("Response Status Code: " + ex.StatusCode);
                //            sr.WriteLine("Error Code: " + ex.ErrorCode);
                //            sr.WriteLine("Error Type: " + ex.ErrorType);
                //            sr.WriteLine("Request ID: " + ex.RequestId);
                //            sr.WriteLine("XML: " + ex.XML);
                //        }
                //    }
                //    sr.WriteLine();

                //    // Print the number of Amazon S3 Buckets.
                //    var s3Client = AWSClientFactory.CreateAmazonS3Client(
                //        appConfig["AWSAccessKey"],
                //        appConfig["AWSSecretKey"]
                //        );

                //    try
                //    {
                //        ListBucketsResponse response = s3Client.ListBuckets();
                //        int numBuckets = 0;
                //        if (response.Buckets != null &&
                //            response.Buckets.Count > 0)
                //        {
                //            numBuckets = response.Buckets.Count;
                //        }
                //        sr.WriteLine("You have " + numBuckets + " Amazon S3 bucket(s) in the US Standard region.");
                //    }
                //    catch (AmazonS3Exception ex)
                //    {
                //        if (ex.ErrorCode != null && (ex.ErrorCode.Equals("InvalidAccessKeyId") ||
                //            ex.ErrorCode.Equals("InvalidSecurity")))
                //        {
                //            sr.WriteLine("Please check the provided AWS Credentials.");
                //            sr.WriteLine("If you haven't signed up for Amazon S3, please visit http://aws.amazon.com/s3");
                //        }
                //        else
                //        {
                //            sr.WriteLine("Caught Exception: " + ex.Message);
                //            sr.WriteLine("Response Status Code: " + ex.StatusCode);
                //            sr.WriteLine("Error Code: " + ex.ErrorCode);
                //            sr.WriteLine("Request ID: " + ex.RequestId);
                //            sr.WriteLine("XML: " + ex.XML);
                //        }
                //    }
                //    sr.WriteLine("Press any key to continue...");
            }
            return sb.ToString();
        }
    }
}