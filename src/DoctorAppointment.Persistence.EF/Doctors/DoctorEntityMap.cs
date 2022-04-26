using DoctorAppointment.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorAppointment.Persistence.EF.Doctors
{
    public class DoctorEntityMap : IEntityTypeConfiguration<Doctor>
    {
        public void Configure(EntityTypeBuilder<Doctor> builder)
        {
            builder.ToTable("Doctors");

            builder.HasKey(_ => _.Id);

            builder.Property(_ => _.Id)
                .ValueGeneratedOnAdd();

            builder.Property(_ => _.NationalCode)
                .IsRequired()
                .HasMaxLength(10);

            builder.Property(_ => _.FirstName)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(_ => _.LastName)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(_ => _.Field)
                .IsRequired()
                .HasMaxLength(50);
        }
    }
}
