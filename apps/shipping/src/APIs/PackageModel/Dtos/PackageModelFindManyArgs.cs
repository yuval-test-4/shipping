using Microsoft.AspNetCore.Mvc;
using Shipping.APIs.Common;
using Shipping.Infrastructure.Models;

namespace Shipping.APIs.Dtos;

[BindProperties(SupportsGet = true)]
public class PackageModelFindManyArgs : FindManyInput<PackageModel, PackageModelWhereInput> { }
