
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Escalon
{
    /// <summary>
    /// Manager used to load and keep track of assets used in the game 
    /// </summary>
    public interface IAssetManager : IAspect
    {
        public const string k_instantiateAsset = "IAssetManager.InstantiateAsset";
        public const string k_destroyAsset = "IAssetManager.DestroyAsset";
        
        bool IsLoading();
        Task<T> LoadAssetAsync<T>(string assetAddress) where T : class;
        Task<List<T>> LoadAssetsAsync<T>(string assetAddress) where T : class;
        Task<byte[]> GetImageBytes(string assetAddress);
        Task<string> LoadTextAssetAsync(string assetAddress);
        Task<List<string>> LoadTextAssetsAsync(string assetAddress);
        Task<string[]> LoadAssetPathsAsync(string assetLabel);
        T LoadAsset<T>(string assetAddress) where T : class;
        void ReleaseAssets(params string[] assets);
        T InstantiateAsset<T>(string assetAddress, object parent = null) where T : class;
        Task<T> InstantiateAssetAsync<T>(string assetAddress, object parent = null);
    }
}
