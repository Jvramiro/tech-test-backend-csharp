﻿using System.ComponentModel.DataAnnotations;

namespace TestJrAPI.Models {
    public class Entity {
        [Key]
        public Guid Id { get; set; }
        public DateTime CriadoEm { get; set; }
        public DateTime EditadoEm { get; set; }
        public Guid CriadoPor { get; set; }
        public Guid EditadoPor { get; set; }
        public bool Ativo { get; set; }
    }
}
