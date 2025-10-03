
using Grocery.Core.Interfaces.Repositories;
using Grocery.Core.Interfaces.Services;
using Grocery.Core.Models;

namespace Grocery.Core.Services
{
    public class BoughtProductsService : IBoughtProductsService
    {
        private readonly IGroceryListItemsRepository _groceryListItemsRepository;
        private readonly IClientRepository _clientRepository;
        private readonly IProductRepository _productRepository;
        private readonly IGroceryListRepository _groceryListRepository;
        public BoughtProductsService(IGroceryListItemsRepository groceryListItemsRepository, IGroceryListRepository groceryListRepository, IClientRepository clientRepository, IProductRepository productRepository)
        {
            _groceryListItemsRepository=groceryListItemsRepository;
            _groceryListRepository=groceryListRepository;
            _clientRepository=clientRepository;
            _productRepository=productRepository;
        }
        //public List<BoughtProducts> Get(int? productId)
        //{

        //    if (productId is null) return [];

        //    Product? product = _productRepository.Get((int)productId);

        //    List<GroceryListItem> groceryListItems = [.. _groceryListItemsRepository.GetAll().Where(p => p.ProductId == productId)];

        //    if (groceryListItems.Count == 0 || product is null) return [];

        //    List<BoughtProducts> boughtProductsList = [];
        //    GroceryList? groceryList;
        //    Client? client;

        //    foreach (GroceryListItem groceryListItem in groceryListItems)
        //    {

        //        groceryList = _groceryListRepository.Get(groceryListItem.GroceryListId);
        //        if (groceryList is null) continue;
        //        client = _clientRepository.Get(groceryList.ClientId);
        //        if (client is null) continue;
        //        boughtProductsList.Add(new BoughtProducts(client, groceryList, product));
        //    }
        //    return boughtProductsList;
        //}
        public List<BoughtProducts> Get(int? productId)
        {
            if (productId is null) return [];

            Product? product = _productRepository.Get(productId.Value);
            if (product is null) return [];

            List<GroceryListItem> groceryListItems = [.. _groceryListItemsRepository.GetAll().Where(p => p.ProductId == productId)];

            if (groceryListItems.Count == 0) return [];

            List<BoughtProducts> boughtProductsList = groceryListItems
                .Select(item =>
                {
                    GroceryList? groceryList = _groceryListRepository.Get(item.GroceryListId);
                    if (groceryList is null) return null;

                    Client? client = _clientRepository.Get(groceryList.ClientId);
                    if (client is null) return null;

                    return new BoughtProducts(client, groceryList, product);
                })
                .Where(bp => bp is not null)
                .ToList()!;


            return boughtProductsList;
        }
    }
}
