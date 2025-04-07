using System;
using System.Text;
using System.IO;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using System.Security.Cryptography;
using AutoMapper;
using Inframanager;
using Inframanager.BLL;
using InfraManager.CrossPlatform.WebApi.Contracts.Common;
using InfraManager.DAL;
using InfraManager.DAL.Accounts;
using Microsoft.Extensions.Logging;

namespace InfraManager.BLL.Accounts
{
    internal class UserAccountBLL : IUserAccountBLL, ISelfRegisteredService<IUserAccountBLL>
    {
        private static string SecurityKey = "jd89$32#90JHgwj$";
        private static byte[] IV = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 };
        
        private readonly ILogger<UserAccountBLL> _logger;
        private readonly IGuidePaggingFacade<UserAccount, UserAccountListItem> _guidePaggingFacade;
        private readonly IMapper _mapper;
        private readonly ICurrentUser _currentUser;
        private readonly IGetEntityBLL<int, UserAccount, UserAccountDetails> _detailsBLL;
        private readonly IInsertEntityBLL<UserAccount, UserAccountData> _insertEntityBLL;
        private readonly IModifyEntityBLL<int, UserAccount, UserAccountData, UserAccountDetails> _modifyEntityBLL;
        private readonly IRemoveEntityBLL<int, UserAccount> _removeEntityBLL;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidatePermissions<UserAccount> _permissionValidator;
        private readonly IRepository<UserAccount> _repository;

        public UserAccountBLL(
            ILogger<UserAccountBLL> logger,
            IUnitOfWork unitOfWork,
            ICurrentUser currentUser,
            IInsertEntityBLL<UserAccount, UserAccountData> insertEntityBLL,
            IModifyEntityBLL<int, UserAccount, UserAccountData, UserAccountDetails> modifyEntityBLL,
            IRemoveEntityBLL<int, UserAccount> removeEntityBLL,
            IGetEntityBLL<int, UserAccount, UserAccountDetails> detailsBLL,
            IGuidePaggingFacade<UserAccount, UserAccountListItem> guidePaggingFacade,
            IMapper mapper,
            IValidatePermissions<UserAccount> permissionValidator,
            IRepository<UserAccount> repository) 
        {
            _logger = logger;
            _repository = repository;
            _unitOfWork = unitOfWork;
            _guidePaggingFacade = guidePaggingFacade;
            _mapper = mapper;
            _permissionValidator = permissionValidator;
            _currentUser = currentUser;
            _detailsBLL = detailsBLL;
            _insertEntityBLL = insertEntityBLL;
            _removeEntityBLL = removeEntityBLL;
            _modifyEntityBLL = modifyEntityBLL;
        }

        public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            await _removeEntityBLL.RemoveAsync(id, cancellationToken);
            await _unitOfWork.SaveAsync(cancellationToken);

            _logger.LogInformation($"User (ID = {_currentUser.UserId}) deleted {typeof(UserAccount).Name} (id = {id}).");
        }

        public async Task<UserAccountDetails> AddAsync(UserAccountData dataModel, CancellationToken cancellationToken = default)
        {
            var userAccount = await _insertEntityBLL.CreateAsync(dataModel, cancellationToken);
            
            EncryptDetails(userAccount);
            
            await _unitOfWork.SaveAsync(cancellationToken);
            _logger.LogInformation($"User (ID = {_currentUser.UserId}) inserted new {typeof(UserAccount).Name} (id = {userAccount.ID})");

            return _mapper.Map<UserAccountDetails>(userAccount);
        }

        public async Task<UserAccountDetails> UpdateAsync(int id, UserAccountData dataModel, CancellationToken cancellationToken = default)
        {
            var oldUserAccount = await _detailsBLL.DetailsAsync(id, cancellationToken);
            var userAccount = await _modifyEntityBLL.ModifyAsync(id, dataModel, cancellationToken);
            
            EncryptDetails(userAccount, oldUserAccount);
            
            await _unitOfWork.SaveAsync(cancellationToken);
            _logger.LogInformation($"User (ID = {_currentUser.UserId}) successfully updated {typeof(UserAccount).Name} (id = {id})");
            
            return _mapper.Map<UserAccountDetails>(userAccount);
        }

        public async Task<UserAccountDetails> DetailsAsync(int id, bool isDecoded, CancellationToken cancellationToken = default)
        {
            var userAccountDetails = await _detailsBLL.DetailsAsync(id, cancellationToken);
            
            if(isDecoded)
            {
                //userAccountDetails decode
                DecryptDetails(userAccountDetails);
            }

            return userAccountDetails;
        }

        public async Task<UserAccountDetails[]> ListAsync(UserAccountFilter filter, CancellationToken cancellationToken = default)
        {
            await _permissionValidator.UserHasPermissionAsync(_currentUser.UserId, ObjectAction.ViewDetailsArray,
                cancellationToken);
            
            Expression<Func<UserAccount, bool>> searchPredicate = string.IsNullOrEmpty(filter.SearchString) ? null :
                (x => x.Name.ToLower().Contains(filter.SearchString.ToLower())
                || x.Login.ToLower().Contains(filter.SearchString.ToLower())
                //|| x.Tags.Any(t => t.Name.ToLower().Contains(filter.SearchString.ToLower())) // TODO Will need search by the Tags
                );

            var result =
                await _guidePaggingFacade.GetPaggingAsync(filter, _repository.Query(), searchPredicate,
                    cancellationToken);

            var userAccountDetails = _mapper.Map<UserAccountDetails[]>(result);
            
            if (filter.IsDecoded)
            {
                userAccountDetails.ForEach(ua => DecryptDetails(ua));
            }

            return userAccountDetails;
        }

        private string Encrypt(string plainText)
        {
            if (string.IsNullOrWhiteSpace(plainText))
            {
                return plainText;
            }
            
            byte[] encrypted;

            // Create an Aes object
            // with the specified key and IV
            using (Aes aesAlg = Aes.Create())
            {
                var keyArray = Encoding.UTF8.GetBytes(SecurityKey);
                
                aesAlg.Key = keyArray;
                aesAlg.IV = IV;

                // Create an encryptor to perform the stream transform.
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for encryption.
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            //Write all data to the stream.
                            swEncrypt.Write(plainText);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }

            return Convert.ToBase64String(encrypted);
        }

        private string Decrypt(string cipherText)
        {
            if (string.IsNullOrWhiteSpace(cipherText))
            {
                return cipherText;
            }

            var encodedText = Convert.FromBase64String(cipherText);
            // Declare the string used to hold
            // the decrypted text.
            string plaintext = null;

            // Create an Aes object
            // with the specified key and IV.
            using (Aes aesAlg = Aes.Create())
            {
                var keyArray = Encoding.UTF8.GetBytes(SecurityKey);

                aesAlg.Key = keyArray;
                aesAlg.IV = IV;

                // Create a decryptor to perform the stream transform.
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for decryption.
                using (MemoryStream msDecrypt = new MemoryStream(encodedText))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            // Read the decrypted bytes from the decrypting stream
                            // and place them in a string.
                            plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }
            }

            return plaintext;
        }

        private void EncryptDetails(UserAccount newUserAccount, UserAccountDetails oldUserAccount = null)
        {
            if(oldUserAccount != null) 
            {
                if(oldUserAccount.Password != newUserAccount.Password)
                {
                    newUserAccount.Password = Encrypt(newUserAccount.Password);
                }
            }
            else
            {
                newUserAccount.Password = Encrypt(newUserAccount.Password);
            }
            
            if (oldUserAccount != null)
            {
                if(oldUserAccount.SSH_Passphrase != newUserAccount.SSH_Passphrase)
                {
                    newUserAccount.SSH_Passphrase = Encrypt(newUserAccount.SSH_Passphrase);
                }
            }
            else
            {
                newUserAccount.SSH_Passphrase = Encrypt(newUserAccount.SSH_Passphrase);
            }
            
            if (oldUserAccount != null)
            {
                if(oldUserAccount.SSH_PrivateKey != newUserAccount.SSH_PrivateKey)
                {
                    newUserAccount.SSH_PrivateKey = Encrypt(newUserAccount.SSH_PrivateKey);
                }
            }
            else
            {
                newUserAccount.SSH_PrivateKey = Encrypt(newUserAccount.SSH_PrivateKey);
            }

            if (oldUserAccount != null)
            {
                if(oldUserAccount.AuthenticationKey != newUserAccount.AuthenticationKey)
                {
                    newUserAccount.AuthenticationKey = Encrypt(newUserAccount.AuthenticationKey);
                }
            }
            else
            {
                newUserAccount.AuthenticationKey = Encrypt(newUserAccount.AuthenticationKey);
            }
            
            if (oldUserAccount != null)
            {
                if(oldUserAccount.PrivacyKey != newUserAccount.PrivacyKey)
                {
                    newUserAccount.PrivacyKey = Encrypt(newUserAccount.PrivacyKey);
                }
            }
            else
            {
                newUserAccount.PrivacyKey = Encrypt(newUserAccount.PrivacyKey);
            }
        }

        private void DecryptDetails(UserAccountDetails userAccountDetails)
        {
            userAccountDetails.Password = Decrypt(userAccountDetails.Password);
            userAccountDetails.SSH_Passphrase = Decrypt(userAccountDetails.SSH_Passphrase);
            userAccountDetails.SSH_PrivateKey = Decrypt(userAccountDetails.SSH_PrivateKey);
            userAccountDetails.AuthenticationKey = Decrypt(userAccountDetails.AuthenticationKey);
            userAccountDetails.PrivacyKey = Decrypt(userAccountDetails.PrivacyKey);
        }
    }
}
