using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using OnlinePizza.Data;
using OnlinePizza.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlinePizza.Services
{
    public class CalculateCartTotalService
    {

        public int CartTotal(Cart cart)
        {
            int totalSum = 0;
            
            foreach (var dish in cart.CartItems)
            {
                totalSum += dish.Price;
            }

            return totalSum;
        }
    }
}
