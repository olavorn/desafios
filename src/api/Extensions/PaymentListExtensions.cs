using api.Model;
using api.Models.EntityModel;
using api.Models.Enums;
using api.Models.IntegrationModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace api.Extensions
{
    public static class PaymentListExtensions
    {
        public static IQueryable<Payment> WhereDateFrom(this IQueryable<Payment> paymentList, DateTime? date)
        {
            if (!date.HasValue)
                return paymentList;
            return paymentList.Where(q=> q.CreatedAt >= date.Value);
        }

        public static IQueryable<Payment> WhereDateUntil(this IQueryable<Payment> paymentList, DateTime? date)
        {
            if (!date.HasValue)
                return paymentList;
            return paymentList.Where(q => q.CreatedAt <= date.Value);
        }

        public static IQueryable<Payment> WherePayerName(this IQueryable<Payment> paymentList, string payerName)
        {
            if (payerName == null)
                return paymentList;
            return paymentList.Where(q => q.CardName.Contains(payerName));
        }

        public static IQueryable<Payment> WherePaidDateFrom(this IQueryable<Payment> paymentList, DateTime? date)
        {
            if (!date.HasValue)
                return paymentList;
            return paymentList.Where(q => q.PaidAt >= date.Value);
        }

        public static IQueryable<Payment> WherePaidDateUntil(this IQueryable<Payment> paymentList, DateTime? date)
        {
            if (!date.HasValue)
                return paymentList;
            return paymentList.Where(q => q.PaidAt <= date.Value);
        }

        public static IQueryable<Payment> WherePaymentResult(this IQueryable<Payment> paymentList, PaymentResponse? response)
        {
            if (!response.HasValue)
                return paymentList;
            return paymentList.Where(q => q.Result <= response.Value);
        }

        public static IQueryable<Payment> WhereStatus(this IQueryable<Payment> paymentList, PaymentStatus? status)
        {
            if (!status.HasValue)
                return paymentList;
            return paymentList.Where(q => q.Status <= status.Value);
        }

        public static IIncludableQueryable<Payment, List<Instalment> > IncludeInstalments(this IQueryable<Payment> paymentList)
        {
            return paymentList.Include( q=> q.Instalments );
        }
        

    }
}
