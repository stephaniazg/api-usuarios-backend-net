﻿namespace ApiUsuarios.Interfaz
{
    public interface IEncryptionService
    {
        string Encrypt(string plainText);
        string Decrypt(string cipherText);
    }
}
