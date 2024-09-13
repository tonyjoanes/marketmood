using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Domain;
using api.Persistence;
using MediatR;
using MongoDB.Driver;

namespace api.Application.Products
{
    public class List
    {
        public class Query : IRequest<List<Product>>
        {
        }

        public class Handler : IRequestHandler<Query, List<Product>>
        {
            private ProductRepository _productRepository;

            public Handler(ProductRepository productRepository)
            {
                _productRepository = productRepository;
            }

            public async Task<List<Product>> Handle(Query request, CancellationToken cancellationToken)
            {
                return await _productRepository.GetAllAsync();
            }
        }
    }
}