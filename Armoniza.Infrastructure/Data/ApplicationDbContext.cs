using System;
using System.Collections.Generic;
using Armoniza.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Armoniza.Infrastructure.Infrastructure.Data;

public partial class ApplicationDbContext : DbContext
{
    public ApplicationDbContext()
    {
    }
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Admin> Admin { get; set; }

    public virtual DbSet<apartado> apartados { get; set; }
    
    public virtual DbSet<categoria> categoria { get; set; }

    public virtual DbSet<detalleApartado> detalleApartados { get; set; }

    public virtual DbSet<detalleUsuario> detalleUsuarios { get; set; }

    public virtual DbSet<grupo> grupos { get; set; }

    public virtual DbSet<grupoDirector> grupoDirectors { get; set; }

    public virtual DbSet<instrumento> instrumentos { get; set; }

    public virtual DbSet<tipoUsuario> tipoUsuarios { get; set; }

    public virtual DbSet<usuario> usuarios { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=Armoniza_Test;Username=postgres;Password=@ViewedPuma92662");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Admin>(entity =>
        {
            entity.HasKey(e => e.id).HasName("Admin_pkey");

            entity.ToTable("Admin", "usuarios");

            entity.Property(e => e.nombre_completo).HasMaxLength(100);
            entity.Property(e => e.password).HasMaxLength(100);
            entity.Property(e => e.telefono).HasMaxLength(10);
            entity.Property(e => e.username).HasMaxLength(50);
        });

        modelBuilder.Entity<apartado>(entity =>
        {
            entity.HasKey(e => e.id).HasName("apartado_pkey");

            entity.ToTable("apartado", "apartados");

            entity.Property(e => e.activo)
                .HasDefaultValue(true)
                .HasComment("Si el apartado esta activo (Aun no devuelto) o ya se libero");
            entity.Property(e => e.fechaDado)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasComment("Fecha en la que se entrego el instrumento")
                .HasColumnType("timestamp without time zone");
            entity.Property(e => e.fechaRegreso)
                .HasComment("fecha estipulada para devolver el instrumento, máximo un semestre")
                .HasColumnType("timestamp without time zone");
            entity.Property(e => e.fechaRetornado).HasColumnType("timestamp without time zone");
            entity.Property(e => e.idUsuario).HasComment("Id del usuario que pidió el apartado");

            entity.HasOne(d => d.idUsuarioNavigation).WithMany(p => p.apartado)
                .HasForeignKey(d => d.idUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_apartado_usuario");
        });

        modelBuilder.Entity<categoria>(entity =>
        {
            entity.HasKey(e => e.id).HasName("categoria_pkey");

            entity.ToTable("categoria", "catalogo", tb => tb.HasComment("Categoria para instrumentos, viento, percursion, etc"));

            entity.Property(e => e.categoria1).HasMaxLength(50);
            entity.Property(e => e.eliminado).HasDefaultValue(false);
        });

        modelBuilder.Entity<detalleApartado>(entity =>
        {
            entity.HasKey(e => e.id).HasName("detalleApartado_pkey");

            entity.ToTable("detalleApartado", "apartados");

            entity.HasIndex(e => new { e.idInstrumento, e.idApartado }, "U_instrumento_apartado").IsUnique();

            entity.HasOne(d => d.idApartadoNavigation).WithMany(p => p.detalleApartado)
                .HasForeignKey(d => d.idApartado)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_detalle_apartado");

            entity.HasOne(d => d.idInstrumentoNavigation).WithMany(p => p.detalleApartado)
                .HasForeignKey(d => d.idInstrumento)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_detalle_instrumento");
        });

        modelBuilder.Entity<detalleUsuario>(entity =>
        {
            entity.HasKey(e => e.id).HasName("detalleUsuario_pkey");

            entity.ToTable("detalleUsuario", "usuarios", tb => tb.HasComment("Tabla para relacionar a los aspirantes con su responsable (director).\nLa logica para saber si es o no aspirante y director aun no esta o se hara en el backend (probablemente ambos)"));

            entity.HasIndex(e => new { e.idDirector, e.idAspirante }, "U_director_usuario").IsUnique();

            entity.HasOne(d => d.idAspiranteNavigation).WithMany(p => p.detalleUsuarioidAspiranteNavigation)
                .HasForeignKey(d => d.idAspirante)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_detalle_aspirante");

            entity.HasOne(d => d.idDirectorNavigation).WithMany(p => p.detalleUsuarioidDirectorNavigation)
                .HasForeignKey(d => d.idDirector)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_detalle_director");
        });

        modelBuilder.Entity<grupo>(entity =>
        {
            entity.HasKey(e => e.id).HasName("grupo_pkey");

            entity.ToTable("grupo", "catalogo", tb => tb.HasComment("Grupos artisticos"));

            entity.Property(e => e.eliminado)
                .HasDefaultValue(false)
                .HasComment("soft delete");
            entity.Property(e => e.grupo1)
                .HasMaxLength(50)
                .HasColumnName("grupo");
        });

        modelBuilder.Entity<grupoDirector>(entity =>
        {
            entity.HasKey(e => e.id).HasName("grupoDirector_pkey");

            entity.ToTable("grupoDirector", "catalogo", tb => tb.HasComment("Tabla que relaciona a un grupo con su respectivo director, un director puede tener mas de un grupo (No ha pasado, pero es posible)\nFalta la logica para verificar si el usuario es un director, o se hara en el backend (probablemente ambos)"));

            entity.HasIndex(e => new { e.idGrupo, e.idDirector }, "U_director_grupo").IsUnique();

            entity.Property(e => e.idDirector).HasComment("id del director al que se le relacionara con un grupo, un director puede tener mas de un grupo (Pero es muy raro, y no ha ocurrido hasta ahora)");
            entity.Property(e => e.idGrupo).HasComment("id del grupo artistico, se relacionara con su director");

            entity.HasOne(d => d.idDirectorNavigation).WithMany(p => p.grupoDirector)
                .HasForeignKey(d => d.idDirector)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_detalle_director");

            entity.HasOne(d => d.idGrupoNavigation).WithMany(p => p.grupoDirector)
                .HasForeignKey(d => d.idGrupo)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_detalle_grupo");
        });

        modelBuilder.Entity<instrumento>(entity =>
        {
            entity.HasKey(e => e.codigo).HasName("instrumento_pkey");

            entity.ToTable("instrumento", "catalogo", tb => tb.HasComment("Tabla de instrumentos, tiene fk con categoría de instrumentos"));

            entity.HasIndex(e => e.ocupado, "idx_ocupado").HasAnnotation("Npgsql:StorageParameter:deduplicate_items", "true");

            entity.Property(e => e.codigo)
                .ValueGeneratedNever()
                .HasComment("Es el \"id\" del instrumento, pero debe llamarse codigo por como se manejan los codigos del instrumento en itson centro. No debe der autoincremental, porque los instrumentos ya tienen un codigo");
            entity.Property(e => e.eliminado)
                .HasDefaultValue(false)
                .HasComment("Soft delete, por si hay algun error de dedo");
            entity.Property(e => e.estuche).HasComment("Si el instrumento tiene o no estuche");
            entity.Property(e => e.funcional)
                .HasDefaultValue(true)
                .HasComment("Si el instrumento sirve o no (De no hacerlo, se deberia mandar a arreglar, pero eso sale del alcance)");
            entity.Property(e => e.idCategoria).HasComment("Id de la categoria a la que pertenece");
            entity.Property(e => e.nombre).HasMaxLength(100);
            entity.Property(e => e.ocupado)
                .HasDefaultValue(false)
                .HasComment("Esta siendo usado o no");

            entity.HasOne(d => d.idCategoriaNavigation).WithMany(p => p.instrumento)
                .HasForeignKey(d => d.idCategoria)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_instrumento_categoria");
        });

        modelBuilder.Entity<tipoUsuario>(entity =>
        {
            entity.HasKey(e => e.id).HasName("tipoUsuario_pkey");

            entity.ToTable("tipoUsuario", "catalogo", tb => tb.HasComment("Representa los tipos de usuario (director, instructor, aspirantes y alumno grupo artistico (IGA))"));

            entity.Property(e => e.capacidadInstrumentos).HasComment("Cuantos instrumentos puede pedir el usuario segun su tipo");
            entity.Property(e => e.eliminado).HasDefaultValue(false);
            entity.Property(e => e.tipo).HasMaxLength(20);
        });

        modelBuilder.Entity<usuario>(entity =>
        {
            entity.HasKey(e => e.id).HasName("usuario_pkey");

            entity.ToTable("usuario", "usuarios", tb => tb.HasComment("Tabla de usuarios,  hay 4 tipos de usuarios, aspirantes, alumnos grupo artístico, instructores y directores. Cada uno tiene un idTipoUsuario. El campo de grupoId es opcional para algunos tipos de usuarios, por lo que es nulo. "));

            entity.HasIndex(e => e.correo, "U_usuario_correo").IsUnique();

            entity.HasIndex(e => e.telefono, "U_usuario_telefono").IsUnique();

            entity.HasIndex(e => e.nombreCompleto, "idx_nombre").HasAnnotation("Npgsql:StorageParameter:deduplicate_items", "true");

            entity.Property(e => e.correo)
                .HasMaxLength(100)
                .HasComment("Obligatorio, por este medio se mandaran avisos para que regrese el instrumento");
            entity.Property(e => e.eliminado).HasDefaultValue(false);
            entity.Property(e => e.idGrupo).HasComment("Opcional, ya que los aspirantes no tienen grupo, asimismo, los directores no están dentro, sino que los dirigen, pero esa relación se hace en la tabla GrupoDirector, del esquema catalogo. ");
            entity.Property(e => e.nombreCompleto)
                .HasMaxLength(150)
                .HasComment("Nombre completo, todo minusculas");
            entity.Property(e => e.telefono)
                .HasMaxLength(10)
                .HasComment("opcional");

            entity.HasOne(d => d.idGrupoNavigation).WithMany(p => p.usuario)
                .HasForeignKey(d => d.idGrupo)
                .HasConstraintName("fk_usuario_grupo");

            entity.HasOne(d => d.idTipoNavigation).WithMany(p => p.usuario)
                .HasForeignKey(d => d.idTipo)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_usuario_tipo");
        });
        modelBuilder.HasSequence("aspirante_id_seq", "usuarios");
        modelBuilder.HasSequence("iga_id_seq", "usuarios");
        modelBuilder.HasSequence("instructor_id_seq", "usuarios");

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
