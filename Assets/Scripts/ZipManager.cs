using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;
using System.IO;
using ICSharpCode.SharpZipLib.Zip;

// Using ICSharpCode.SharpZipLib.Zip >> GNU GENERAL PUBLIC LICENSE Version 2

public class ZipManager

{
    /// <summary>
    /// Ư�� ������ ZIP���� ����
    /// </summary>
    /// <param name="targetFolderPath">���� ��� ���� ���</param>
    /// <param name="zipFilePath">������ ZIP ���� ���</param>
    /// <param name="password">���� ��ȣ</param>
    /// <param name="isDeleteFolder">���� ���� ����</param>
    /// <returns>���� ���� ����</returns>
    public static bool ZipFiles(string targetFolderPath, string zipFilePath, string password, bool isDeleteFolder)
    {
        bool retVal = false;

        // ������ �����ϴ� ��쿡�� ����.
        if (Directory.Exists(targetFolderPath))
        {
            // ���� ��� ������ ���� ���.
            ArrayList ar = GenerateFileList(targetFolderPath);

            // ���� ��� ���� ����� ���� + 1
            int TrimLength = (Directory.GetParent(targetFolderPath)).ToString().Length + 1;

            // find number of chars to remove. from orginal file path. remove '\'
            FileStream ostream;
            byte[] obuffer;
            string outPath = zipFilePath;

            // ZIP ��Ʈ�� ����.
            ZipOutputStream oZipStream = new ZipOutputStream(File.Create(outPath));
            try
            {
                // �н����尡 �ִ� ��� �н����� ����.
                if (password != null && password != String.Empty)
                    oZipStream.Password = password;

                oZipStream.SetLevel(9); // ��ȣȭ ����.(�ִ� ����)

                ZipEntry oZipEntry;

                foreach (string Fil in ar)
                {
                    oZipEntry = new ZipEntry(Fil.Remove(0, TrimLength));
                    oZipStream.PutNextEntry(oZipEntry);

                    // ������ ���.
                    if (!Fil.EndsWith(@"/"))
                    {
                        ostream = File.OpenRead(Fil);
                        obuffer = new byte[ostream.Length];
                        ostream.Read(obuffer, 0, obuffer.Length);
                        oZipStream.Write(obuffer, 0, obuffer.Length);
                    }
                }

                retVal = true;
            }
            catch
            {
                retVal = false;

                // ������ �� ��� ���� �ߴ� ������ ����.
                if (File.Exists(outPath))
                    File.Delete(outPath);
            }
            finally
            {
                // ���� ����.
                oZipStream.Finish();
                oZipStream.Close();
            }


            // ���� ������ ���� ��� ���� ����.
            if (isDeleteFolder)
                try
                {
                    Directory.Delete(targetFolderPath, true);
                }
                catch { }
        }
        return retVal;
    }

    /// <summary>
    /// ����, ���� ��� ����
    /// </summary>
    /// <param name="Dir">���� ���</param>
    /// <returns>����, ���� ���(ArrayList)</returns>
    private static ArrayList GenerateFileList(string Dir)
    {
        ArrayList fils = new ArrayList();
        bool Empty = true;

        // ���� ���� ���� �߰�.
        foreach (string file in Directory.GetFiles(Dir))
        {
            fils.Add(file);
            Empty = false;
        }

        if (Empty)
        {
            // ������ ����, ������ ���� ��� �ڽ��� ���� �߰�.
            if (Directory.GetDirectories(Dir).Length == 0)
                fils.Add(Dir + @"/");
        }

        // ���� �� ���� ���.
        foreach (string dirs in Directory.GetDirectories(Dir))
        {
            // �ش� ������ �ٽ� GenerateFileList ��� ȣ��
            foreach (object obj in GenerateFileList(dirs))
            {
                // �ش� ���� ���� ����, ���� �߰�.
                fils.Add(obj);
            }
        }

        return fils;
    }

    /// <summary>
    /// ���� ���� Ǯ��
    /// </summary>
    /// <param name="zipFilePath">ZIP���� ���</param>
    /// <param name="unZipTargetFolderPath">���� Ǯ ���� ���</param>
    /// <param name="password">���� ��ȣ</param>
    /// <param name="isDeleteZipFile">zip���� ���� ����</param>
    /// <returns>���� Ǯ�� ���� ���� </returns>
    public static bool UnZipFiles(string zipFilePath, string unZipTargetFolderPath, string password, bool isDeleteZipFile)
    {
        bool retVal = false;

        // ZIP ������ �ִ� ��츸 ����.
        if (File.Exists(zipFilePath))
        {
            // ZIP ��Ʈ�� ����.
            ZipInputStream zipInputStream = new ZipInputStream(File.OpenRead(zipFilePath));

            // �н����尡 �ִ� ��� �н����� ����.
            if (password != null && password != String.Empty)
                zipInputStream.Password = password;

            try
            {
                ZipEntry theEntry;
                // �ݺ��ϸ� ������ ������.
                while ((theEntry = zipInputStream.GetNextEntry()) != null)
                {
                    // ����
                    string directoryName = Path.GetDirectoryName(theEntry.Name);
                    string fileName = Path.GetFileName(theEntry.Name); // ����

                    // ���� ����
                    Directory.CreateDirectory(unZipTargetFolderPath + directoryName);

                    // ���� �̸��� �ִ� ���
                    if (fileName != String.Empty)
                    {
                        // ���� ��Ʈ�� ����.(���ϻ���)
                        FileStream streamWriter = File.Create((unZipTargetFolderPath + theEntry.Name));

                        int size = 2048;
                        byte[] data = new byte[2048];

                        // ���� ����
                        while (true)
                        {
                            size = zipInputStream.Read(data, 0, data.Length);

                            if (size > 0)
                                streamWriter.Write(data, 0, size);
                            else
                                break;
                        }

                        // ���Ͻ�Ʈ�� ����
                        streamWriter.Close();
                    }
                }
                retVal = true;
            }
            catch
            {
                retVal = false;
            }
            finally
            {
                // ZIP ���� ��Ʈ�� ����
                zipInputStream.Close();
            }

            // ZIP���� ������ ���� ��� ���� ����.
            if (isDeleteZipFile)
                try
                {
                    File.Delete(zipFilePath);
                }
                catch { }
        }
        return retVal;
    }
}