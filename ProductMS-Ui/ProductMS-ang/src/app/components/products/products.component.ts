import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { ProductService, Product } from '../../shared/services/product.service'; 

@Component({
  selector: 'app-products',
  templateUrl: './products.component.html',
  styleUrls: ['./products.component.css']
})
export class ProductsComponent implements OnInit {

  @ViewChild('empModal') empModal: ElementRef | undefined;
  productList: Product[] = [];
  selectedProduct: Product | null = null;

  constructor(private productService: ProductService) {}

  ngOnInit(): void {
    this.getAllProducts();
  }

  openModal() {
    if (this.empModal) {
      this.empModal.nativeElement.style.display = 'block';
    }
  }

  closeModal() {
    if (this.empModal) {
      this.empModal.nativeElement.style.display = 'none';
    }
    this.selectedProduct = null;
    this.clearForm();  
  }

  getAllProducts() {
    this.productService.getProducts().subscribe(
      (products) => {
        console.log("Products fetched:", products);
        this.productList = products;
      },
      (error) => {
        console.error("Error fetching products:", error);
      }
    );
  }

  openEditModal(product: Product) {
    this.selectedProduct = { ...product }; 
    this.setFormValues(product);
    this.openModal();
  }
  setFormValues(product: Product) {
    (document.getElementById('productName') as HTMLInputElement).value = product.name;
    (document.getElementById('productDescription') as HTMLTextAreaElement).value = product.description;
    (document.getElementById('productPrice') as HTMLInputElement).value = product.price.toString();
    (document.getElementById('productCategory') as HTMLInputElement).value = product.category;
    (document.getElementById('createdBy') as HTMLInputElement).value = product.createdBy;
  }

  
  saveProduct() {
    const productData: Product = {
      id: this.selectedProduct?.id ?? undefined,
      name: (document.getElementById('productName') as HTMLInputElement).value,
      description: (document.getElementById('productDescription') as HTMLTextAreaElement).value,
      price: parseFloat((document.getElementById('productPrice') as HTMLInputElement).value),
      category: (document.getElementById('productCategory') as HTMLInputElement).value,
      createdBy: (document.getElementById('createdBy') as HTMLInputElement).value
    };

    if (this.selectedProduct?.id) {
  
      this.productService.updateProduct(this.selectedProduct.id, productData).subscribe(
        (updatedProduct) => {
          console.log("Product updated:", updatedProduct);
          this.getAllProducts();
          this.closeModal();
        },
        (error) => {
          console.error("Error updating product:", error);
        }
      );
    } else {
 
      this.productService.createProduct(productData).subscribe(
        (newProduct) => {
          console.log("Product added:", newProduct);
          this.getAllProducts();
          this.closeModal();
        },
        (error) => {
          console.error("Error adding product:", error);
        }
      );
    }
  }

  deleteProduct(id: number) {
    this.productService.deleteProduct(id).subscribe(
      () => {
        console.log("Product deleted");
        this.getAllProducts();
      },
      (error) => {
        console.error("Error deleting product:", error);
      }
    );
  }


  clearForm() {
    (document.getElementById('productName') as HTMLInputElement).value = '';
    (document.getElementById('productDescription') as HTMLTextAreaElement).value = '';
    (document.getElementById('productPrice') as HTMLInputElement).value = '';
    (document.getElementById('productCategory') as HTMLInputElement).value = '';
    (document.getElementById('createdBy') as HTMLInputElement).value = '';
  }
}
